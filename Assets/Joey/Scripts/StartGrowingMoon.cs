using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGrowingMoon : MonoBehaviour
{
    public GrowMoon growMoon;

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player") {
            growMoon.StartGrowing();
            Destroy(this);
        }
    }
}
