using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class EventOnTrigger : MonoBehaviour
{
    public UnityEvent eventToTriggerOnEnter;
	public UnityEvent eventToTriggerOnExit;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			eventToTriggerOnEnter.Invoke ();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			eventToTriggerOnExit.Invoke ();
		}
	}
}
