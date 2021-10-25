using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralAudioSettings
{
	public const float pitchRandomRange = 0.3f;
	public const float normalPitch = 1f;

	public static float GetRandomPitch()
	{
		return 1f + Random.Range (-pitchRandomRange, pitchRandomRange);
	}
}
