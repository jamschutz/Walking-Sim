using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jsch 
{
	public class DialogAudio : MonoBehaviour 
	{
		[System.Serializable]
		public class AudioSettings
		{
			[Range(0f, 1f)]
			public float volume;
			[Range(0f, 1f)]
			public float spatialBlend;
		}

		public AudioClip[] clips;
		public int numAudioSources;
		public AudioSettings audioSettings;

		AudioSource[] audioSources;
		int currentAudioIndex;

		void Awake()
		{
			audioSources = new AudioSource[numAudioSources];

			for (int i = 0; i < audioSources.Length; i++) 
			{
				audioSources [i] = gameObject.AddComponent<AudioSource> ();
				audioSources[i].volume = audioSettings.volume;
				audioSources[i].spatialBlend = audioSettings.spatialBlend;
				audioSources[i].playOnAwake = false;
			}

			currentAudioIndex = 0;
		}

		public void Play()
		{
			if (audioSources [currentAudioIndex].isPlaying) 
			{
				audioSources [currentAudioIndex].Stop ();
			}

			audioSources [currentAudioIndex].clip = GetRandomClip();
			audioSources [currentAudioIndex].Play ();

			SetNextAudioIndex ();
		}

		AudioClip GetRandomClip()
		{
			int clipID = Random.Range (0, clips.Length);
			return clips [clipID];
		}

		void SetNextAudioIndex ()
		{
			currentAudioIndex++;
			if (currentAudioIndex >= audioSources.Length)
				currentAudioIndex = 0;
		}
	}

}