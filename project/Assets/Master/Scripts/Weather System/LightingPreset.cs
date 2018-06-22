using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LightingPreset : ScriptableObject {

	public string settingName;
	
	public Material skybox;

	public Vector3 sunRotation;
	public float sunIntensity;
	public Color sunColor;

	public Vector3 fillLightRotation;
	public float fillLightIntensity;
	public Color fillLightColor;

	public AudioClip[] transientSounds;
	public float transientVolume = 1;
	public float minSoundDelay;
	public float maxSoundDelay;

}
