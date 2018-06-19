using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RainState {
	none,light,moderate,heavy,thunder
}

public class RainIntensity : MonoBehaviour {

	public RainState rainIntensity;

	public AudioClip lightRain;
	public AudioClip modRain;
	public AudioClip heavyRain;
	public AudioClip thunder;
	public AudioClip noRain;
	
	private ParticleSystem rain;
	private ParticleSystem splash;
	private AudioSource sound;

	private int rainLevel;
	private int currentLevel;
	
	void Start() {
		rain = GetComponent<ParticleSystem>();
		splash = rain.transform.Find("Splashes").GetComponent<ParticleSystem>();
		sound = GetComponent<AudioSource>();

		rainLevel = (int)rainIntensity;
		updateRainIntensity(rainLevel);
		currentLevel = rainLevel;
	}

	void Update() {

		rainLevel = (int)rainIntensity;
		if (currentLevel != rainLevel) {
			currentLevel = rainLevel;
			updateRainIntensity(rainLevel);
		}
		
	}

	void updateRainIntensity(int newIntensityLevel) {

		var rainEmission = rain.emission;
		var splashVel = splash.velocityOverLifetime;

		switch(rainLevel){
			case 0: 
				rainEmission.rate = 0;
				sound.clip = noRain;
				break;
			case 1:
				rainEmission.rate = 50;
				sound.clip = lightRain;
				sound.volume = 1f;
				break;
			case 2:
				rainEmission.rate = 130;
				sound.clip = modRain;
				sound.volume = 0.7f;
				break;
			case 3:
				rainEmission.rate = 350;
				sound.clip = heavyRain;
				sound.volume = 0.5f;
				break;
			case 4:
				rainEmission.rate = 350;
				sound.clip = thunder;
				sound.volume = 0.8f;
				break;
		}
		sound.Play();
		splashVel.yMultiplier = rainLevel * 2;

	}
}
