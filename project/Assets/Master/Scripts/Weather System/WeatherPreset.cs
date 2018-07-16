using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeatherPreset : ScriptableObject {

	/*if you want to add another element to the preset, 
	make sure to also change the environment manager
	so it will update the relevant settings of the proper object */

	[Header("Name")]
	public string settingName;

	[Header("Menu Cube Appearance")]
	public Texture icon;
	public Color blockTint;
	
	[Header("Rain/Weather Particles")]
	public float precipitationIntensity;

	[Header("Wind")]
	public float windIntensity;
	public float windTurbulence;
	public float windPulseMag;
	public float windPulseFreq;

	[Header("Fog")]
	public bool fog;
	public Color fogColor;
	public float fogDensity;
	public FogMode fogMode;

	[Header("Audio: Ambient Sound")]
	public AudioClip ambientSound;
	public float ambientVolume = 1; 


	
}
