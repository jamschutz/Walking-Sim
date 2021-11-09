using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GiraffeCarController : MonoBehaviour
{
    public Transform targetDestination;

    NavMeshAgent navMesh;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.SetDestination(targetDestination.position);
    }
}
