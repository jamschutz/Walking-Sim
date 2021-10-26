using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowMoon : MonoBehaviour
{
    public float scaleSpeed = 1.0f;
    public float endingScaleSpeed = 5.0f;
    public bool growOnAwake = false;
    public float maxSize = 200;

    bool isGrowing;
    Vector3 scaleIncrement;
    Vector3 endingScaleIncrement;
    bool isGrowingFaster;
    bool isEnding;


    void Start()
    {
        isGrowing = growOnAwake;
        isGrowingFaster = false;
        isEnding = false;
        scaleIncrement = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
        endingScaleIncrement = new Vector3(endingScaleSpeed, endingScaleSpeed, endingScaleSpeed);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            isGrowingFaster = true;
        }
    }


    void Update()
    {
        Debug.Log($"is growing: {isGrowing} and is ending: {isEnding}");
        if(isGrowing && transform.localScale.x < maxSize)
        {
            transform.localScale += scaleIncrement * Time.deltaTime;
        }
        else if(isGrowing && isEnding)
        {            
            transform.localScale += endingScaleIncrement * Time.deltaTime;
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


    public void StartEnding()
    {
        isEnding = true;
    }
}
