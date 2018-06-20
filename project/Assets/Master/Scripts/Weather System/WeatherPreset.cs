using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeatherPreset : ScriptableObject {

	public string nameOfSetting;
	
	public float rainIntensity;

	public float windIntensity;
	public float windTurbulence;
	public float windPulseMag;
	public float windPulseFreq;

	
}
