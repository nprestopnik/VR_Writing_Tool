using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	public Light sunLight;
	public Light fillLight;
	public WindZone windZone;
	public ParticleSystem fog;
	public GameObject fireflies;

	private EnvironmentAudioManager audioManager;

	void Start() {
		WeatherSystemManager.instance.SetSceneEnvironmentManager(gameObject);
		audioManager = GetComponent<EnvironmentAudioManager>();
	}

	public void SetWeather(WeatherPreset newWeather) {
		WeatherSystemManager.instance.rainEmission.rateOverTime = newWeather.rainIntensity;

		var fogEmission = fog.emission;
		fogEmission.rateOverTime = newWeather.fogAmount;

		windZone.windMain = newWeather.windIntensity;
		windZone.windTurbulence = newWeather.windTurbulence;
		windZone.windPulseMagnitude = newWeather.windPulseMag;
		windZone.windPulseFrequency = newWeather.windPulseFreq;

		WeatherSystemManager.instance.ambientSource.clip = newWeather.ambientSound;
		WeatherSystemManager.instance.ambientSource.volume = newWeather.ambientVolume;
		WeatherSystemManager.instance.ambientSource.Play();
	}

	public void SetLighting(LightingPreset newLighting) {
		RenderSettings.skybox = newLighting.skybox;

		sunLight.transform.rotation = Quaternion.Euler(newLighting.sunRotation);
		sunLight.intensity = newLighting.sunIntensity;
		sunLight.color = newLighting.sunColor;

		fillLight.transform.rotation = Quaternion.Euler(newLighting.fillLightRotation);
		fillLight.intensity = newLighting.fillLightIntensity;
		fillLight.color = newLighting.fillLightColor;

		audioManager.transientSoundClips = newLighting.transientSounds;
		audioManager.transientVolume = newLighting.transientVolume;
		audioManager.minDelay = newLighting.minSoundDelay;
		audioManager.maxDelay = newLighting.maxSoundDelay;

		if(fireflies) {
			if(newLighting.fireflies) {
				fireflies.SetActive(true);
			} else {
				fireflies.SetActive(false);
			}
		}
	}

}
	
