using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using UnityEditor;
using UnityEngine;

public enum EnvironmentCubeType {
	mood,weather
}

public class WeatherMoodContainer : MonoBehaviour {

	public EnvironmentCubeType type;

	public LightingPreset moodPreset;
	public WeatherPreset weatherPreset;

	Transform cubeParent;
	[HideInInspector]
	public MeshRenderer blockMesh;
	[HideInInspector]
	public MeshRenderer iconMesh;

	public GameObject cubeTween;
	[HideInInspector]
	public TransformTweenBehaviour tween;
	[HideInInspector]
	public Transform tweenHidden;
	[HideInInspector]
	public Transform tweenVisible;

	void Awake() {
		cubeParent = transform.Find("Cube");
		blockMesh = cubeParent.Find("block mesh").gameObject.GetComponent<MeshRenderer>();
		iconMesh = cubeParent.Find("icon mesh").gameObject.GetComponent<MeshRenderer>();

		tween = cubeTween.GetComponent<TransformTweenBehaviour>();
		tweenHidden = cubeTween.transform.Find("Hidden");
		tweenVisible = cubeTween.transform.Find("Visible");
	}

	//as it is, a given cube will only have one preset, either mood or weather
	//so only one of these functions will work on a given cube, so be careful
	public void SetMood() {
		WeatherSystemManager.instance.environment.SetLighting(moodPreset);
	}

	public void SetWeather() {
		WeatherSystemManager.instance.environment.SetWeather(weatherPreset);
	}
}


