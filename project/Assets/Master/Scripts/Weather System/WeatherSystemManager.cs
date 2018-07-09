using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour {

	public GameObject environmentManager;

	public WeatherPreset[] weatherPresets;
    public LightingPreset[] lightingPresets;


	public LightingPreset dawn;
	public LightingPreset dusk;
	public LightingPreset bright;
	public LightingPreset overcast;
	public LightingPreset storms;
	public LightingPreset night;
	public WeatherPreset clear;
	public WeatherPreset gusty;
	public WeatherPreset mist;
	public WeatherPreset raining;
	public WeatherPreset pouring;
	public WeatherPreset deluge;


	private EnvironmentManager environment;

    // [Header("Testing Triggers")]
    // public bool clear = false;
    // public bool windy = false;
	// public bool rainy = false;

	// public bool day = false;
	// public bool dusk = false;
	// public bool late = false;

	
    void Start () {
		environmentManager = GameObject.Find("EnvironmentManager");
		environment = environmentManager.GetComponent<EnvironmentManager>();
	}
	
	void Update () {

        //IF STATEMENTS FOR TESTING ONLY
        // if(clear) {
        //     environment.SetWeather(Array.Find(weatherPresets, weatherPreset => weatherPreset.settingName == "Clear"));
		// 	clear = false;
        // }
		// if(windy) {
		// 	environment.SetWeather(weatherPresets[1]);
		// 	windy = false;
		// }
		// if(rainy) {
		// 	environment.SetWeather(weatherPresets[2]);
		// 	rainy = false;
		// }

		// if(day) {
		// 	environment.SetLighting(Array.Find(lightingPresets, lightingPreset => lightingPreset.settingName == "Midday"));
		// 	day = false;
		// }
		// if(dusk) {
		// 	environment.SetLighting(lightingPresets[1]);
		// 	dusk = false;
		// }
		// if(late) {
		// 	environment.SetLighting(lightingPresets[2]);
		// 	late = false;
		// }


	}

	//public functions for cubes to call

	//mood
	public void SetDawn() {

	}
	public void SetDusk() {

	}
	public void SetBright() {

	}
	public void SetOvercast() {

	}
	public void SetStorms() {

	}
	public void SetNight() {

	}

	//weather
	public void SetClear() {

	}
	public void SetGusty() {

	}
	public void SetMist() {

	}
	public void SetRaining() {

	}
	public void SetPouring() {

	}
	public void SetDeluge() {

	}

}
