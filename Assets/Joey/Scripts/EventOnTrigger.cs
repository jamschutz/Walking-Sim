using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class EventOnTrigger : MonoBehaviour
{
    public UnityEvent eventToTriggerOnEnter;
	public UnityEvent eventToTriggerOnExit;
	public float waitTime = 0;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			Invoke("InvokeEnter", waitTime);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			Invoke("InvokeExit", waitTime);
		}
	}


	void InvokeEnter()
	{
		eventToTriggerOnEnter.Invoke();
	}

	void InvokeExit()
	{
		eventToTriggerOnExit.Invoke();
	}
}
