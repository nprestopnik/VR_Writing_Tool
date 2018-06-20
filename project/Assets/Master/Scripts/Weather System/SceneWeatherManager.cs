using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWeatherManager : MonoBehaviour {

	public ParticleSystem rain;
	private ParticleSystem.EmissionModule rainEmission;

	public GameObject sun;
	private Light sunLight;
	public GameObject fill;
	private Light fillLight;

	public GameObject wind;
	private WindZone windZone;


	void Start() {
		rainEmission = rain.emission;
		windZone = wind.GetComponent<WindZone>();
		sunLight = sun.GetComponent<Light>();
		fillLight = fill.GetComponent<Light>();
	}

	void Update() {
	
	}

	public void SetWeather(WeatherPreset newWeather) {
		rainEmission.rateOverTime = newWeather.rainIntensity;

		windZone.windMain = newWeather.windIntensity;
		windZone.windTurbulence = newWeather.windTurbulence;
		windZone.windPulseMagnitude = newWeather.windPulseMag;
		windZone.windPulseFrequency = newWeather.windPulseFreq;
	}

	public void SetLighting(LightingPreset newLighting) {
		RenderSettings.skybox = newLighting.skybox;

		sunLight.transform.position = newLighting.sunLocation.position;
		sunLight.transform.rotation = newLighting.sunLocation.rotation;
		sunLight.intensity = newLighting.sunIntensity;
		sunLight.color = newLighting.sunColor;

		fillLight.transform.position = newLighting.fillLightLocation.position;
		fillLight.transform.rotation = newLighting.fillLightLocation.rotation;
		fillLight.intensity = newLighting.fillLightIntensity;
		fillLight.color = newLighting.fillLightColor;

	}

}
	
