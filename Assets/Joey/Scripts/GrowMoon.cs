using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowMoon : MonoBehaviour
{
    public float scaleSpeed = 1.0f;
    public bool growOnAwake = false;
    public float maxSize = 200;

    bool isGrowing;
    Vector3 scaleIncrement;
    bool isGrowingFaster;


    void Start()
    {
        isGrowing = growOnAwake;
        isGrowingFaster = false;
        scaleIncrement = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            isGrowingFaster = true;
        }
    }


    void Update()
    {
        if(isGrowing && transform.localScale.x < maxSize)
        {
            transform.localScale += scaleIncrement * Time.deltaTime;
        }

        if(isGrowingFaster)
        {
            // scaleIncrement *= 1.0f * Time.deltaTime;
        }
    }


    public void StartGrowing()
    {
        isGrowing = true;
    }
}
