using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	[Header("Environment Element References")]
	public Light sunLight;
	public Light fillLight;
	public WindZone windZone;
	public GameObject otherVisualEffects;
	
	[Header("Rain/Main Weather Particles with Positioning")]
	public ParticleSystem precipitation;
	[Tooltip("The particles will track the player; determine where they should be relative to the head.")]
	public Vector3 particleOffsetFromHead;

	[Header("Ambient Sound Source with Positioning")]
	public AudioSource ambientSoundSource;
	[Tooltip("The ambient source will track the player; determine where it should be relative to the head.")]
	public Vector3 ambientOffsetFromHead;

	[Header("Presets for Mood and Weather with menu size for scene")]

	[Tooltip("NUMBER OF PRESETS PER CATEGORY <= 2 X MAX CUBES PER ROW")]
	public int maxCubesPerMenuRow = 3;
	[Tooltip("Mood preset objects are called Lighting Presets")]
	public LightingPreset[] moodPresets;
	public WeatherPreset[] weatherPresets;

	private EnvironmentAudioManager audioManager;

	void Start() {
		WeatherSystemManager.instance.SetSceneEnvironmentManager(gameObject);
		audioManager = GetComponent<EnvironmentAudioManager>();

		CreateWeatherMoodCubes.instance.CreateMoodCubes(moodPresets, maxCubesPerMenuRow);
		CreateWeatherMoodCubes.instance.CreateWeatherCubes(weatherPresets, maxCubesPerMenuRow);
	}

	void Update() {
		precipitation.transform.position = PlayerController.instance.head.position + particleOffsetFromHead;
		ambientSoundSource.transform.position = PlayerController.instance.head.position + ambientOffsetFromHead;
	}

	public void SetWeather(WeatherPreset newWeather) {
		var particleEmission = precipitation.emission;
		particleEmission.rateOverTime = newWeather.precipitationIntensity;

	
		windZone.windMain = newWeather.windIntensity;
		windZone.windTurbulence = newWeather.windTurbulence;
		windZone.windPulseMagnitude = newWeather.windPulseMag;
		windZone.windPulseFrequency = newWeather.windPulseFreq;
		
		RenderSettings.fog = newWeather.fog;
		if(newWeather.fog) {
			RenderSettings.fogColor = newWeather.fogColor;
			RenderSettings.fogDensity = newWeather.fogDensity;
			RenderSettings.fogMode = newWeather.fogMode;
		}

		ambientSoundSource.clip = newWeather.ambientSound;
		ambientSoundSource.volume = newWeather.ambientVolume;
		ambientSoundSource.Play();
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

		otherVisualEffects.SetActive(newLighting.otherVisualEffects);
	}

}
	
