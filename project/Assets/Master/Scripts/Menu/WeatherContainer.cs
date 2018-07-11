using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherContainer : MonoBehaviour {

	public WeatherPreset preset;

	Transform cubeParent;
	[HideInInspector]
	public MeshRenderer blockMesh;
	[HideInInspector]
	public MeshRenderer iconMesh;

	public void GetMeshes() {
		cubeParent = transform.Find("Cube");
		blockMesh = cubeParent.Find("block mesh").gameObject.GetComponent<MeshRenderer>();
		iconMesh = cubeParent.Find("icon mesh").gameObject.GetComponent<MeshRenderer>();
	}

	public void SetWeather() {
		WeatherSystemManager.instance.environment.SetWeather(preset);
	}
}
