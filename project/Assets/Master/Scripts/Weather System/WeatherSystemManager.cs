/*
Weather System Manager
holds on to the current scene environment manager and makes sure the menu has the right cubes 
	and is hooked up to the right things
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour {

	public static WeatherSystemManager instance;

	[HideInInspector]
	public GameObject environmentManager;
	[HideInInspector]
	public EnvironmentManager environment;

	void Awake() {
		instance = this;
	}
	


	//this is called when a scene's environment manager loads in; it sets the system environment manager and creates menu cubes
    public void SetSceneEnvironmentManager(GameObject sceneEnvironment) {
		environmentManager = sceneEnvironment;
		environment = environmentManager.GetComponent<EnvironmentManager>();

		CreateWeatherMoodCubes.instance.CreateCubes(environment.maxCubesPerMenuRow, EnvironmentCubeType.mood, moodPresets: environment.moodPresets);
		CreateWeatherMoodCubes.instance.CreateCubes(environment.maxCubesPerMenuRow, EnvironmentCubeType.weather, weatherPresets: environment.weatherPresets);
	}

}
