using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EventOnTrigger : MonoBehaviour
{
    public ChangeSkyboxTintOverTime changeSkybox;

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player") {
            changeSkybox.BeginTransition();
            Destroy(this);
        }
    }
}
