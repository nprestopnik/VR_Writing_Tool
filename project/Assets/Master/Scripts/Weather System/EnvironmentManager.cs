using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	public ParticleSystem rain;
	private ParticleSystem.EmissionModule rainEmission;

	public Light sunLight;
	public Light fillLight;
	public WindZone windZone;

	public EnvironmentAudioManager audioManager;


	void Start() {
		rainEmission = rain.emission;
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

		sunLight.transform.rotation = Quaternion.Euler(newLighting.sunRotation);
		sunLight.intensity = newLighting.sunIntensity;
		sunLight.color = newLighting.sunColor;

		fillLight.transform.rotation = Quaternion.Euler(newLighting.fillLightRotation);
		fillLight.intensity = newLighting.fillLightIntensity;
		fillLight.color = newLighting.fillLightColor;

	}

}
	
