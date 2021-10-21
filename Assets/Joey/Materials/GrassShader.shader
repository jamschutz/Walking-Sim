 // Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Terrain/Grass"
{
    Properties
    {
		[Header(Shading)]
        _TopColor("Top Color", Color) = (1,1,1,1)
		_BottomColor("Bottom Color", Color) = (1,1,1,1)
		_TranslucentGain("Translucent Gain", Range(0,1)) = 0.5
		
		[Header(Transform)]
		_BendAmount("Bend amount", Range(0, 1)) = 0.2
		_BladeWidth("Blade width", Float) = 0.05
		_BladeWidthRandom("Blade width random distribution", Float) = 0.02
		_BladeHeight("Blade height", Float) = 0.5
		_BladeHeightRandom("Blade height random distribution", Float) = 0.3
		
		[Header(Tessellation)]
		_TessellationUniform("Tessellation Amount", Range(1, 64)) = 1
		
		[Header(Wind)]
		_WindDistortionMap("Wind distortion map", 2D) = "white" {}
		_WindFrequency("Wind frequency", Vector) = (0.05, 0.05, 0, 0)
		_WindStrength("Wind strength", Float) = 1
		_WindTime("Wind power of over time", Range(0, 2)) = 1

        [Header(Blade Curve)]
        _BladeForward("Blade Forward Amount", Float) = 0.38
        _BladeCurve("Blade Curvature Amount", Range(1, 4)) = 2

		[Header(Lighting)]
		_SpecularColor("Specular Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Float) = 1
    }

	CGINCLUDE
	#include "UnityCG.cginc"
	#include "Autolight.cginc"
	#include "CustomTessellation.cginc"

// ------------ Properties -------------------- //
    #define BLADE_SEGMENTS 3

	float _BendAmount;

	float _BladeWidth;
	float _BladeWidthRandom;
	float _BladeHeight;
	float _BladeHeightRandom;

	sampler2D _WindDistortionMap;
	float4    _WindDistortionMap_ST;
	float2    _WindFrequency;
	float     _WindStrength;
	float	  _WindTime;

    float _BladeForward;
    float _BladeCurve;
// -------------------------------------------- //

	struct geometryOutput 
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
        float3 normal : NORMAL;
        float3 worldPos : TEXCOORD1;
		float height : TEXCOORD2;
	};

	// Simple noise function, sourced from http://answers.unity.com/answers/624136/view.html
	// Extended discussion on this function can be found at the following link:
	// https://forum.unity.com/threads/am-i-over-complicating-this-random-function.454887/#post-2949326
	// Returns a number in the 0...1 range.
	float rand(float3 co)
	{
		return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
	}

	// Construct a rotation matrix that rotates around the provided axis, sourced from:
	// https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33
	float3x3 AngleAxis3x3(float angle, float3 axis)
	{
		float c, s;
		sincos(angle, s, c);

		float t = 1 - c;
		float x = axis.x;
		float y = axis.y;
		float z = axis.z;

		return float3x3(
			t * x * x + c, t * x * y - s * z, t * x * z + s * y,
			t * x * y + s * z, t * y * y + c, t * y * z - s * x,
			t * x * z - s * y, t * y * z + s * x, t * z * z + c
			);
	}

	// takes in a position, converts it from object to clip space, and stores in geometryOutput
	geometryOutput geoVertToClipPos(float3 pos, float3 worldPos, float height, float2 uv, float3x3 transformMatrix)
	{
		geometryOutput o;
		o.pos = UnityObjectToClipPos(pos);
		o.uv = uv;
        o.normal = mul(transformMatrix, float3(0, 0, 1));
        o.worldPos = pos;
		o.height = height;

		return o;
	}

    // takes in a grass blade transform data, and generates the triangle strip for it
    geometryOutput generateGrassVertex(float3 position, float3 worldPos, float width, float height, float forward, float2 uv, float3x3 transformMatrix)
    {
        // note that in tangent space, "UP" is on the Z (not Y) axis
        float3 tangentPoint = float3(width, forward, height); 

        float3 localPosition = position + mul(transformMatrix, tangentPoint);
        return geoVertToClipPos(localPosition, worldPos, height, uv, transformMatrix);
    }

	[maxvertexcount(BLADE_SEGMENTS * 2 + 1)]
	void geo(triangle vertexOutput input[3] : SV_POSITION, inout TriangleStream<geometryOutput> triStream)
	{
		geometryOutput o;
		float3 pos = input[0].vertex.xyz;
		float width   = (rand(pos.xzy) * 2 - 1) * _BladeWidthRandom  + _BladeWidth;
		float height  = (rand(pos.zyx) * 2 - 1) * _BladeHeightRandom + _BladeHeight;
        float forward = rand(pos.yyz) * _BladeForward;

		// because we want the grass to point in the normal direction (tangent space), 
		// we need to calculate in tangent space, then map to object space
		float3 vNormal   = input[0].normal;
		float4 vTangent  = input[0].tangent;
		float3 vBinormal = cross(vNormal, vTangent) * vTangent.w;
		// construct transformation matrix
		float3x3 tangentToLocal = float3x3(
			vTangent.x, vBinormal.x, vNormal.x,
			vTangent.y, vBinormal.y, vNormal.y,
			vTangent.z, vBinormal.z, vNormal.z
		);

		// we'll randomly rotate each blade -- construct rotation matrix
		// we use pos for the random seed so it's the same each frame. note that the
		// axis of rotation is the Z axis, because in tangent space "UP" is on the Z axis
		float3x3 grassRotationMatrix = AngleAxis3x3(rand(pos) * UNITY_TWO_PI, float3(0, 0, 1));
		float3x3 grassBendMatrix     = AngleAxis3x3(rand(pos.zzx) * _BendAmount * UNITY_PI * 0.5, float3(-1, 0, 0));

		// calculate wind
		float2 uv = pos.xz * _WindDistortionMap_ST.xy + _WindDistortionMap_ST.zw + _WindFrequency * (_Time.y * _WindTime);
		float2 windSample = (tex2Dlod(_WindDistortionMap, float4(uv, 0, 0)).xy * 2 - 1) * _WindStrength;
		float3 wind = normalize(float3(windSample.x, windSample.y, 0));
		float3x3 windRotation = AngleAxis3x3(UNITY_PI * windSample, wind);

		// build transformation matrix in tangent space
		float3x3 transformationMatrix = mul(mul(mul(tangentToLocal, windRotation), grassRotationMatrix), grassBendMatrix);
		// we don't want to apply the same transformation to the base, 
		// because it will cause the bottom to lift off the ground
		float3x3 grassBaseTransform = mul(tangentToLocal, grassRotationMatrix);

        for(int i = 0; i < BLADE_SEGMENTS; i++)
        {
            float t = i / (float)BLADE_SEGMENTS;

            // as we move up the blade, width decreases and height increases
            float segmentWidth   = width * (1 - t);
            float segmentHeight  = height * t;
            float segmentForward = pow(t, _BladeCurve) * forward;
            
            // create two vertices at this level
            triStream.Append(generateGrassVertex(pos, input[0].worldPosition, segmentWidth,  segmentHeight, segmentForward, float2(0, t),   transformationMatrix)); 
            triStream.Append(generateGrassVertex(pos, input[0].worldPosition, -segmentWidth, segmentHeight, segmentForward, float2(1, t),   transformationMatrix));
        }

        // add final vertex at the top
        // triStream.Append(generateGrassVertex(pos, input[0].worldPosition, 0, height, forward, float2(0.5, 1), transformationMatrix));
	}
	ENDCG

    SubShader
    {
		Cull Off

        Pass
        {
			Tags
			{
				"RenderType" = "Opaque"
				"LightMode" = "ForwardBase"
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma geometry geo
			#pragma hull hull
			#pragma domain domain
			#pragma target 4.6
            
			#include "Lighting.cginc"

			float4 _TopColor;
			float4 _BottomColor;
			float _TranslucentGain;

			fixed4 _SpecularColor;
			float _Shininess; 

			float4 frag (geometryOutput input, fixed facing : VFACE) : SV_Target
            {	
                // normal/view directions
                float3 normal  = input.normal;
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - input.worldPos.xyz);
                // light vars
                float3 lightDir;
                float attenuation;

                // directional light
                if(_WorldSpaceLightPos0.w == 0.0)
                {
                    attenuation = 1.0;
                    lightDir = normalize(_WorldSpaceLightPos0.xyz);
                }
                else 
                {
                    float3 fragToLightDirection = _WorldSpaceLightPos0.xyz - input.worldPos.xyz;
                    float distanceFromLight = length(fragToLightDirection);
                    attenuation = 1.0 / distanceFromLight;
                    lightDir = normalize(fragToLightDirection);
                }

                // apply lighting
                float3 diffuse = attenuation * _LightColor0.xyz * saturate(dot(normal, lightDir));
				float3 specular = attenuation * _SpecularColor * pow(saturate(dot(reflect(-lightDir, normal), viewDir)), _Shininess);

                float3 lightFinal = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuse;

				return lerp(_BottomColor, _TopColor, input.uv.y) * float4(lightFinal, 1);
            }
            ENDCG
        }

        Pass
        {
			Cull Off 

			Tags
			{
				"LightMode" = "ForwardAdd"
			}

            Blend One One
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma geometry geo
			#pragma hull hull
			#pragma domain domain
			
            #pragma target 4.6
            #pragma multi_compile_fwdadd
            
			#include "Lighting.cginc"

			float4 _TopColor;
			float4 _BottomColor;
			float _TranslucentGain;

			fixed4 _SpecularColor;
			float _Shininess;

			float4 frag (geometryOutput input, fixed facing : VFACE) : SV_Target
            {	
            // ------- lighting -------- //
                float3 normal  = input.normal;
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - input.worldPos.xyz);

                float3 lightDir;
            #if defined(DIRECTION)
                lightDir = normalize(_WorldSpaceLightPos0.xyz);
            #elif defined(POINT) || defined(SPOT)
                lightDir    = normalize(_WorldSpaceLightPos0.xyz - input.worldPos.xyz);
			#else
				lightDir = float3(1,1,1);
            #endif

                UNITY_LIGHT_ATTENUATION(attenuation, 0, input.worldPos.xyz);

                float3 diffuse = attenuation * _LightColor0.rgb * saturate(abs(dot(normal, lightDir))) * input.height;
				float3 specular = attenuation * _SpecularColor * pow(saturate(dot(reflect(-lightDir, normal), viewDir)), _Shininess);
                float3 lightFinal = diffuse + UNITY_LIGHTMODEL_AMBIENT;
            // ------------------------- //

				return lerp(_BottomColor, _TopColor, input.uv.y) * float4(lightFinal, 1);
            }
            ENDCG
        }
    }
}