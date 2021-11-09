using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCameraFade : MonoBehaviour
{
    public void DestroyIt()
    {
        Destroy(GameObject.Find("CameraFade"));
    }
}
