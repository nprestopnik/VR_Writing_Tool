using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour {

	public WeatherPreset[] weatherPresets;
    public LightingPreset[] lightingPresets;

    public GameObject weatherManager;
	private SceneWeatherManager sceneWeather;

    [Header("Testing Triggers")]
    public bool clear = false;
    public bool windy = false;
	public bool rainy = false;
	
    void Start () {
		weatherManager = GameObject.Find("WeatherManager");
		sceneWeather = weatherManager.GetComponent<SceneWeatherManager>();
	}
	
	void Update () {

        //IF STATEMENTS FOR TESTING ONLY
        if(clear) {
            sceneWeather.SetWeather(weatherPresets[0]);
			clear = false;
        }
		if(windy) {
			sceneWeather.SetWeather(weatherPresets[1]);
			windy = false;
		}
		if(rainy) {
			sceneWeather.SetWeather(weatherPresets[2]);
			rainy = false;
		}


	}

}
