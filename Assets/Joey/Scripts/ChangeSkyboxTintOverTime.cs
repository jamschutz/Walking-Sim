using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyboxTintOverTime : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public float transitionTime;
    public bool transitionOnAwake;

    bool isTransitioning;
    float counter;

    void Start()
    {
        isTransitioning = transitionOnAwake;
        counter = 0;
        SetSkyboxTint(startColor);
    }


    void Update()
    {
        if(isTransitioning) {
            Debug.Log("transitioning!");
            Color newColor = Color.Lerp(startColor, endColor, counter / transitionTime);
            SetSkyboxTint(newColor);

            counter += Time.deltaTime;
        }
    }


    public void BeginTransition()
    {
        isTransitioning = true;
    }


    void SetSkyboxTint(Color tint)
    {
        RenderSettings.skybox.SetColor("_TintColor", tint);
    }
}
