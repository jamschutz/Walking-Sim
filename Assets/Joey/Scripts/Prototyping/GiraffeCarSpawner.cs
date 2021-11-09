using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiraffeCarSpawner : MonoBehaviour
{
    public float minSpeed = 24.5f;
    public float maxSpeed = 25.5f;

    public float minTimeBetweenSpawns = 1.0f;
    public float maxTimeBetweenSpawns = 2.0f;

    public GameObject giraffeCarPrefab;
    public Transform giraffeCarDestination;

    float timer;
    float waitTime;

    void Start()
    {
        // init wait time
        ResetWaitTime();
    }


    void Update()
    {
        // if time to spawn a new giraffe car, do it
        if(timer >= waitTime) {
            ResetWaitTime();
            SpawnGiraffeCar();
        }

        // increment timer
        timer += Time.deltaTime;
    }


    void ResetWaitTime()
    {
        timer = 0f;
        waitTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }


    void SpawnGiraffeCar()
    {
        // instantiate new car -- set ourselves as the parent
        var giraffeCar = GameObject.Instantiate(giraffeCarPrefab, transform.position, transform.rotation, transform);

        // randomize car speed
        float speed = Random.Range(minSpeed, maxSpeed);
        var navMeshAgent = giraffeCar.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;

        // set destination
        var carController = giraffeCar.GetComponent<GiraffeCarController>();
        carController.targetDestination = giraffeCarDestination;
    }
}
