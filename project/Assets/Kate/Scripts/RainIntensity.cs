using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainIntensity : MonoBehaviour {

	[Range(0,300)]
	public float rainIntensity = 0f;
	public ParticleSystem rain;

	void Update () {
		var emissionMod = rain.emission;
		emissionMod.rate = rainIntensity;
	}
}
