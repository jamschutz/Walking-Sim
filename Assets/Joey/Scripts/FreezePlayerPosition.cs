using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerPosition : MonoBehaviour
{
    FirstPersonDrifter fpsController;

    void Start()
    {
        fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonDrifter>();
    }


    public void FreezePlayer()
    {
        Debug.Log("Freezing player!");
        fpsController.enabled = false;
    }
}
