using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayFootsteps : MonoBehaviour 
{
	public float stepInterval;

	AudioSource audio;
	bool leftFoot;
	float stereoAmount = 0.75f;
	float targetTime;

	void Awake()
	{
		audio = GetComponent<AudioSource> ();
	}

	void Start()
	{
		leftFoot = true;
	}

	void Update()
	{
		if (IsMoving() && Time.time >= targetTime)
		{
			PlayFootstep ();
			targetTime = Time.time + stepInterval;
		}
	}

	void PlayFootstep()
	{
		audio.pitch = GeneralAudioSettings.GetRandomPitch ();
		if (leftFoot)
		{
			audio.panStereo = -stereoAmount;
			audio.Play ();
			leftFoot = false;
		} 
		else
		{
			audio.panStereo = stereoAmount;
			audio.Play ();
			leftFoot = true;
		}
	}


	bool IsMoving()
	{
		return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
	}
}
