using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAudioController : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
    public float minVolume;
    public float maxVolume;
    public bool applySpatialization;
    public AnimationCurve volumeCurve;

    Transform player;
    AudioSource audio;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        SetVolume();

        if(applySpatialization)
            SetSpatialization();
    }


    void SetVolume()
    {
        // get raw distance
        // float distance = Mathf.Abs(Vector3.Distance(player.position, transform.position));
        float distance = Mathf.Abs(player.position.x - transform.position.x);

        // clamp distance between min and max volume
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // convert raw distance to [0,1] value
        float volume = 1.0f - ((distance - minDistance) / (maxDistance - minDistance));

        // set volume accordingly
        audio.volume = volumeCurve.Evaluate(volume);
    }


    void SetSpatialization()
    {
        // get camera rotation along the Y axis only
        // convert [0,360] rotation to [0,1] range
        float cameraYRotation = (player.rotation.eulerAngles.y / 360.0f) % 1.0f;

        // calculate how much we hear in each ear (in range [0,1])
        float rightEarWeight = 1.0f - Mathf.Abs(cameraYRotation - 0.5f) * 2.0f;
        float leftEarWeight = Mathf.Abs(cameraYRotation - 0.5f) * 2.0f;

        // set spatiazation using weights
        audio.panStereo = rightEarWeight - leftEarWeight;
    }
}
