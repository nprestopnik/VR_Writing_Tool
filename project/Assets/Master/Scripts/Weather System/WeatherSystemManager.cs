using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour {

	public static WeatherSystemManager instance;

	public GameObject environmentManager;
	private EnvironmentManager environment;

	[Header("Mood/Lighting Presets")]
	public LightingPreset dawn;
	public LightingPreset dusk;
	public LightingPreset bright;
	public LightingPreset overcast;
	public LightingPreset storms;
	public LightingPreset night;

	[Header("Weather Presets")]
	public WeatherPreset clear;
	public WeatherPreset gusty;
	public WeatherPreset mist;
	public WeatherPreset raining;
	public WeatherPreset pouring;
	public WeatherPreset deluge;

	[Header("Attached Environment Elements")]
	public ParticleSystem rain;
	[HideInInspector]
	public ParticleSystem.EmissionModule rainEmission;
	public AudioSource ambientSource;

	void Awake() {
		instance = this;

		rainEmission = rain.emission;
	}
	
    public void SetSceneEnvironmentManager(GameObject sceneEnvironment) {
		environmentManager = sceneEnvironment;
		environment = environmentManager.GetComponent<EnvironmentManager>();
	}

	//public functions for cubes to call

	//mood
	public void SetDawn() {
		environment.SetLighting(dawn);
	}
	public void SetDusk() {
		environment.SetLighting(dusk);
	}
	public void SetBright() {
		environment.SetLighting(bright);
	}
	public void SetOvercast() {
		environment.SetLighting(overcast);
	}
	public void SetStorms() {
		environment.SetLighting(storms);
	}
	public void SetNight() {
		environment.SetLighting(night);
	}

	//weather
	public void SetClear() {
		environment.SetWeather(clear);
	}
	public void SetGusty() {
		environment.SetWeather(gusty);
	}
	public void SetMist() {
		environment.SetWeather(mist);
	}
	public void SetRaining() {
		environment.SetWeather(raining);
	}
	public void SetPouring() {
		environment.SetWeather(pouring);
	}
	public void SetDeluge() {
		environment.SetWeather(deluge);
	}

}
