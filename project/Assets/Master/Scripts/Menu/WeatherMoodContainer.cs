/*
Weather (or) Mood Cube Container
Purpose: to hold on to the necessary information for a weather or mood cube
 */

using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using UnityEditor;
using UnityEngine;

//the options for type of environment cube
public enum EnvironmentCubeType {
	mood,weather
}

public class WeatherMoodContainer : MonoBehaviour {

	public EnvironmentCubeType type; //is this a weather or a mood container?

	//the cube will only have one of these, either a weather or a lighting preset
	public LightingPreset moodPreset;
	public WeatherPreset weatherPreset;

	Transform cubeParent; //the parent to the objects with the actual cube meshes
	[HideInInspector]
	public MeshRenderer blockMesh; //the mesh that determines the color of the cube
	[HideInInspector]
	public MeshRenderer iconMesh; //the mesh with the cube's icon

	public GameObject cubeTween; //the object with the tween behaviour
	[HideInInspector]
	public TransformTweenBehaviour tween; //the tween
	[HideInInspector]
	public Transform tweenHidden; //the location where the cube will hide - the tween starting location
	[HideInInspector]
	public Transform tweenVisible; //the location where the cube will be visible - the tween ending location

	void Awake() {
		//assign everything! hopefully nobody renames things lol
		cubeParent = transform.Find("Cube");
		blockMesh = cubeParent.Find("block mesh").gameObject.GetComponent<MeshRenderer>();
		iconMesh = cubeParent.Find("icon mesh").gameObject.GetComponent<MeshRenderer>();

		tween = cubeTween.GetComponent<TransformTweenBehaviour>();
		tweenHidden = cubeTween.transform.Find("Hidden");
		tweenVisible = cubeTween.transform.Find("Visible");
	}

	//as it is, a given cube will only have one preset, either mood or weather
	//so only one of these functions will work on a given cube, so be careful
	//but these (obviously) set the environment's mood or weather, respectively
	public void SetMood() {
		WeatherSystemManager.instance.environment.SetLighting(moodPreset);
	}

	public void SetWeather() {
		WeatherSystemManager.instance.environment.SetWeather(weatherPreset);
	}
}


