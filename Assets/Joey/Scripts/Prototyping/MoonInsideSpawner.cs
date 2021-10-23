using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MoonInsideSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Material everythingMaterial;
    public float minScale;
    public float maxScale;
    public int numObjectsToSpawn;


    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player") {
            SpawnObjects();
        }
    }


    void SpawnObjects()
    {
        for(int i = 0; i < numObjectsToSpawn; i++) {
            SpawnRandomObject();
        }
    }


    void SpawnRandomObject()
    {
        // get random everything model
        GameObject prefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // instantiate inside moon
        var obj = GameObject.Instantiate(prefab, transform);

        // set scale
        float scale = Random.Range(minScale, maxScale);
        obj.transform.localScale = new Vector3(scale, scale, scale);

        // set rotation
        obj.transform.localEulerAngles = new Vector3(
            Random.Range(0, 359.0f), Random.Range(0, 359.0f), Random.Range(0, 359.0f)
        );

        // set position
        obj.transform.Translate(new Vector3(
            Random.Range(0.0f, 2.0f), Random.Range(0.0f, 2.0f), Random.Range(0.0f, 2.0f)
        ));

        // set material
        obj.GetComponentInChildren<Renderer>().material = everythingMaterial;
    }
}
