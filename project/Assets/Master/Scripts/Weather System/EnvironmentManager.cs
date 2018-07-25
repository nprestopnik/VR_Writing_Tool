/*
Environment Manager
Purpose: keeps track of the scene-specific environment elements and presets and contains the functions to set them
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnvironmentManager : MonoBehaviour {

	[Header("Environment Element References")]
	public Light sunLight; //the sun/main light in the scene
	public Light fillLight; //the moon/fill light in the scene
	public WindZone windZone; //wind!
	public GameObject otherVisualEffects; //a game object or parent object that contains other effects 
	//things under "other visual effects" should (for now at least) be able to be turned on/off with no other settings
	
	[Header("Rain/Main Weather Particles with Positioning")]
	public ParticleSystem precipitation; //the rain system or other primary weather particle system in the scene
	public bool particlesTrackPlayer; //whether or not the particle system will be attached to the player
	[Tooltip("If the particles will track the player, determine where they should be relative to the head.")]
	public Vector3 particleOffsetFromHead; //where the particles should be relative to the player's head

	[Header("Ambient Sound Source with Positioning")]
	public AudioSource ambientSoundSource; //the source for ambient sound in the scene
	public bool ambientTrackPlayer; //will the ambient sound source be attached to the player
	[Tooltip("If the ambient source will track the player, determine where it should be relative to the head.")]
	public Vector3 ambientOffsetFromHead; //where will the sound source be relative to the head, if tracked

	[Header("Presets for Mood and Weather with menu size for scene")]

	[Tooltip("NUMBER OF PRESETS PER CATEGORY <= 2 X MAX CUBES PER ROW")]
	public int maxCubesPerMenuRow = 3; //the maximum number of cubes that will be allowed per row in this scene
	//the top row of cubes will always fill before the bottom row gets any cubes

	[Tooltip("Mood preset objects are called Lighting Presets")]
	public LightingPreset[] moodPresets; //all of the mood presets available for this scene
	public WeatherPreset[] weatherPresets; //all of the weather presets available for this scene

	private EnvironmentAudioManager audioManager; //the scene's audio manager to take care of playing mood-related transient sounds

	void Start() {
		WeatherSystemManager.instance.SetSceneEnvironmentManager(gameObject);
		audioManager = GetComponent<EnvironmentAudioManager>();
		SetWeather(weatherPresets[0]); //Force the initial weather
		SetLighting(moodPresets[0]); //Force the initial mood
	}

	void Update() {

		//keep things attached to player and at the right offset if they are being tracked 

		if(particlesTrackPlayer) {
			precipitation.transform.position = PlayerController.instance.head.position + particleOffsetFromHead;
		}
		
		if(ambientTrackPlayer){
			ambientSoundSource.transform.position = PlayerController.instance.head.position + ambientOffsetFromHead;
		}
		
	}

	//set the weather elements in the scene according to the given preset
	public void SetWeather(WeatherPreset newWeather) {
		//check to make sure that each element has been referenced before setting its properties
		//then set the appropriate things based on what a preset contains, using the given preset

		print(newWeather);

		if(precipitation) {
			var particleEmission = precipitation.emission;
			particleEmission.rateOverTime = newWeather.precipitationIntensity;
		}
	
		if(windZone) {
			windZone.windMain = newWeather.windIntensity;
			windZone.windTurbulence = newWeather.windTurbulence;
			windZone.windPulseMagnitude = newWeather.windPulseMag;
			windZone.windPulseFrequency = newWeather.windPulseFreq;
		}
		
		RenderSettings.fog = newWeather.fog;
		if(newWeather.fog) {
			RenderSettings.fogColor = newWeather.fogColor;
			RenderSettings.fogDensity = newWeather.fogDensity;
			RenderSettings.fogMode = newWeather.fogMode;
		}

		if(ambientSoundSource) {
			ambientSoundSource.clip = newWeather.ambientSound;
			ambientSoundSource.volume = newWeather.ambientVolume;
			ambientSoundSource.Play();
		}
	}

	//set the lighting elements in the scene according to the given preset
	public void SetLighting(LightingPreset newLighting) {
		
		if(newLighting.skybox) {
			RenderSettings.skybox = newLighting.skybox;
		}
	
		if(sunLight) {
			sunLight.transform.rotation = Quaternion.Euler(newLighting.sunRotation);
			sunLight.intensity = newLighting.sunIntensity;
			sunLight.color = newLighting.sunColor;
		}
		
		if(fillLight) {
			fillLight.transform.rotation = Quaternion.Euler(newLighting.fillLightRotation);
			fillLight.intensity = newLighting.fillLightIntensity;
			fillLight.color = newLighting.fillLightColor;
		}

		if(audioManager) {
			audioManager.transientSoundClips = newLighting.transientSounds;
			audioManager.transientVolume = newLighting.transientVolume;
			audioManager.minDelay = newLighting.minSoundDelay;
			audioManager.maxDelay = newLighting.maxSoundDelay;
		}
		
		if(otherVisualEffects) {
			otherVisualEffects.SetActive(newLighting.otherVisualEffects);
		}
	}

}

	
