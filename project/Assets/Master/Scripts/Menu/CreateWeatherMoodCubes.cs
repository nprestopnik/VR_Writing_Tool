using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Animation;

public enum MenuRow {
	upper,lower
}

[RequireComponent(typeof(MenuHandedness))]
public class CreateWeatherMoodCubes : MonoBehaviour {

	public static CreateWeatherMoodCubes instance;

	public GameObject moodCubePrefab;
	public Transform moodHidden;
	public SubMenu moodSubmenu;

	public GameObject weatherCubePrefab;
	public Transform weatherHidden;
	public SubMenu weatherSubmenu;

	private MenuHandedness cubeLocationSetter;

	void Awake() {
		instance = this;
		cubeLocationSetter = GetComponent<MenuHandedness>();
	}

	public GameObject[] CreateMoodRow(MenuRow row, LightingPreset[] presets) {
		
		GameObject[] cubes = new GameObject[presets.Length];
		for(int i = 0; i < presets.Length; i++) {
			GameObject cube = (GameObject)Instantiate(moodCubePrefab);

			if(row == MenuRow.upper) {
				cube.transform.parent = cubeLocationSetter.moodUpperParent.transform;
			} else {
				cube.transform.parent = cubeLocationSetter.moodLowerParent.transform;
			}
			cube.transform.localPosition = Vector3.zero;
			cube.transform.localRotation = Quaternion.identity;

			GameObject moodCube = cube.transform.Find("Mood Cube").gameObject;
			MoodContainer setter = moodCube.GetComponent<MoodContainer>();
			setter.preset = presets[i];
			setter.Setup();
			setter.blockMesh.materials = presets[i].blockMaterials;
			setter.iconMesh.material = presets[i].iconMaterial;

			setter.tween.startTransform = moodHidden;
			setter.tween.SetTargetToStart();
			//setter.hiddenTween.position = moodHidden.transform.position;

			cubes[i] = cube;
		}
		cubeLocationSetter.SetCubePositions(cubes, row == MenuRow.upper, cubeLocationSetter.GetHandedness() == Handedness.right);
		return cubes;
	}

	public void CreateMoodCubes(LightingPreset[] presets, int maxPerRow) {

		int numUpper;
		int numLower;
		if(presets.Length > maxPerRow) {
			numUpper = maxPerRow;
			numLower = presets.Length - numUpper;
		} else {
			numUpper = presets.Length;
			numLower = 0;
		}

		LightingPreset[] upper = new LightingPreset[numUpper];
		LightingPreset[] lower = new LightingPreset[numLower];

		for(int i = 0; i < presets.Length; i++) {
			if(i < upper.Length) {
				upper[i] = presets[i];
			} else {
				lower[i-upper.Length] = presets[i];
			}
		}
		
		GameObject[] allCubes = new GameObject[presets.Length];
		GameObject[] upperCubes = CreateMoodRow(MenuRow.upper, upper);
		GameObject[] lowerCubes = CreateMoodRow(MenuRow.lower, lower);

		for(int i = 0; i < allCubes.Length; i++) {
			if(i < upperCubes.Length) {
				allCubes[i] = upperCubes[i];
			} else {
				allCubes[i] = lowerCubes[i-upperCubes.Length];
			}
		}
		
		moodSubmenu.cubeTweens = new TransformTweenBehaviour[allCubes.Length];
		for (int i = 0; i < allCubes.Length; i++) {
			moodSubmenu.cubeTweens[i] = allCubes[i].transform.Find("Mood Tween").GetComponent<TransformTweenBehaviour>();
		}
	}

	public GameObject[] CreateWeatherRow(MenuRow row, WeatherPreset[] presets) {
		
		GameObject[] cubes = new GameObject[presets.Length];
		for(int i = 0; i < presets.Length; i++) {
			GameObject cube = (GameObject)Instantiate(weatherCubePrefab);

			if(row == MenuRow.upper) {
				cube.transform.parent = cubeLocationSetter.weatherUpperParent.transform;
			} else {
				cube.transform.parent = cubeLocationSetter.weatherLowerParent.transform;
			}
			cube.transform.localPosition = Vector3.zero;
			cube.transform.localRotation = Quaternion.Euler(0f, 45f, 0f);

			WeatherContainer setter = cube.transform.GetComponentInChildren<WeatherContainer>();
			setter.preset = presets[i];
			setter.GetMeshes();
			setter.blockMesh.materials = presets[i].blockMaterials;
			setter.iconMesh.material = presets[i].iconMaterial;

			cubes[i] = cube;
		}
		cubeLocationSetter.SetCubePositions(cubes, row == MenuRow.upper, cubeLocationSetter.GetHandedness() == Handedness.right);
		return cubes;
	}

	public void CreateWeatherCubes(WeatherPreset[] presets, int maxPerRow) {

		int numUpper;
		int numLower;
		if(presets.Length > maxPerRow) {
			numUpper = maxPerRow;
			numLower = presets.Length - numUpper;
		} else {
			numUpper = presets.Length;
			numLower = 0;
		}

		WeatherPreset[] upper = new WeatherPreset[numUpper];
		WeatherPreset[] lower = new WeatherPreset[numLower];

		for(int i = 0; i < presets.Length; i++) {
			if(i < upper.Length) {
				upper[i] = presets[i];
			} else {
				lower[i-upper.Length] = presets[i];
			}
		}
		
		GameObject[] allCubes = new GameObject[presets.Length];
		GameObject[] upperCubes = CreateWeatherRow(MenuRow.upper, upper);
		GameObject[] lowerCubes = CreateWeatherRow(MenuRow.lower, lower);

		for(int i = 0; i < allCubes.Length; i++) {
			if(i < upperCubes.Length) {
				allCubes[i] = upperCubes[i];
			} else {
				allCubes[i] = lowerCubes[i-upperCubes.Length];
			}
		}
		
		weatherSubmenu.cubeTweens = new TransformTweenBehaviour[allCubes.Length];
		for (int i = 0; i < allCubes.Length; i++) {
			weatherSubmenu.cubeTweens[i] = allCubes[i].transform.Find("Weather Tween").GetComponent<TransformTweenBehaviour>();
		}
	}

}
