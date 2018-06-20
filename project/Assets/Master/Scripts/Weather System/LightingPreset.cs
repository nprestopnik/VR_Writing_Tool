using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LightingPreset : ScriptableObject {

	public string nameOfSetting;
	
	public Material skybox;

	public Transform sunLocation;
	public float sunIntensity;
	public Color sunColor;

	public Transform fillLightLocation;
	public float fillLightIntensity;
	public Color fillLightColor;


}
