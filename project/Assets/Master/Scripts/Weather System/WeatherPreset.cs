using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeatherPreset : ScriptableObject {

	public string settingName;
	
	public float rainIntensity;

	public float windIntensity;
	public float windTurbulence;
	public float windPulseMag;
	public float windPulseFreq;

	public AudioClip ambientSound;
	public float ambientVolume = 1; 

	
}
