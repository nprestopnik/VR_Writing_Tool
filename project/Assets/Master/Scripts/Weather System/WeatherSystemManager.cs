using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour {

	public static WeatherSystemManager instance;

	public GameObject environmentManager;
	public EnvironmentManager environment;

	void Awake() {
		instance = this;
	}
	
    public void SetSceneEnvironmentManager(GameObject sceneEnvironment) {
		environmentManager = sceneEnvironment;
		environment = environmentManager.GetComponent<EnvironmentManager>();

		CreateWeatherMoodCubes.instance.CreateCubes(environment.maxCubesPerMenuRow, EnvironmentCubeType.mood, moodPresets: environment.moodPresets);
		CreateWeatherMoodCubes.instance.CreateCubes(environment.maxCubesPerMenuRow, EnvironmentCubeType.weather, weatherPresets: environment.weatherPresets);
	}

}
