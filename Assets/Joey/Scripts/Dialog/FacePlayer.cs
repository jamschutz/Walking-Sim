using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public bool invertRotation;

    Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        if(player == null) {
            Debug.LogError("Unable to find a Game Object with the tag 'Player'");
        }
    }


    void Update()
    {
        if(player != null) {
            transform.LookAt(player);
            
            if(invertRotation) {
                transform.RotateAround(transform.position, Vector3.up, 180.0f);
            }

            transform.localEulerAngles = new Vector3(
                0, transform.localEulerAngles.y, 0
            );
        }
    }
}
