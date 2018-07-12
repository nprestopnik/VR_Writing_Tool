using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeatherPreset : ScriptableObject {

	[Header("Name")]
	[Tooltip("This isn't really used anywhere but you can name your preset so that's fun")]
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
