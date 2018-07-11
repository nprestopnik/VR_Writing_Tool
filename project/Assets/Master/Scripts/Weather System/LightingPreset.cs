using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LightingPreset : ScriptableObject {

	[Header("Name")]
	[Tooltip("This isn't really used anywhere but you can name your preset so that's fun")]
	public string settingName;

	[Header("Menu Cube Materials")]
	public Material iconMaterial;
	public Material[] blockMaterials;
	
	[Header("Skybox")]
	public Material skybox;

	[Header("Sun/Main Light")]
	public Vector3 sunRotation;
	public float sunIntensity;
	public Color sunColor;

	[Header("Moon/Fill Light")]
	public Vector3 fillLightRotation;
	public float fillLightIntensity;
	public Color fillLightColor;

	[Header("Audio: Transient Sounds")]
	public AudioClip[] transientSounds;
	public float transientVolume = 1;
	public float minSoundDelay;
	public float maxSoundDelay;

	[Header("Other Visual Effects")]
	[Tooltip("Other effects includes fireflies and similar elements that can be turned on/off as one unit.")]
	public bool otherVisualEffects;

}
