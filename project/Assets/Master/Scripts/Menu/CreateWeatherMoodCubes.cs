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

	/*only send one preset array, make sure it matches the type provided otherwise everything will break*/
	public GameObject[] CreateCubeRow(MenuRow row, EnvironmentCubeType type, LightingPreset[] moodPresets = null, WeatherPreset[] weatherPresets = null) {
		
		GameObject[] cubes;
		int rowSize;
		GameObject prefab;
		GameObject upperParent;
		GameObject lowerParent;

		if(type == EnvironmentCubeType.mood) {
			rowSize = moodPresets.Length;
			cubes = new GameObject[rowSize];

			prefab = moodCubePrefab;
			upperParent = cubeLocationSetter.moodUpperParent;
			lowerParent = cubeLocationSetter.moodLowerParent;
		} else {
			rowSize = weatherPresets.Length;
			cubes = new GameObject[rowSize];

			prefab = weatherCubePrefab;
			upperParent = cubeLocationSetter.weatherUpperParent;
			lowerParent = cubeLocationSetter.weatherLowerParent;
		}

		GameObject[] visiblePoints = new GameObject[cubes.Length];
		
		for(int i = 0; i < rowSize; i++) {
			
			GameObject cube = (GameObject)Instantiate(prefab);

			GameObject menuCube = cube.transform.Find("Menu Cube").gameObject;
			WeatherMoodContainer setter = menuCube.GetComponent<WeatherMoodContainer>();

			if(row == MenuRow.upper) {
				cube.transform.parent = upperParent.transform;
			} else {
				cube.transform.parent = lowerParent.transform;
			}

			cube.transform.localPosition = Vector3.zero;
			cube.transform.localRotation = Quaternion.identity;

			if(type == EnvironmentCubeType.mood) {
				setter.moodPreset = moodPresets[i];
				
				setter.blockMesh.materials[0].SetColor("_Color", moodPresets[i].blockTint);
				setter.iconMesh.material.SetTexture("_MainTex", moodPresets[i].icon);

				setter.tweenHidden.position = moodHidden.position;
			} else {
				setter.weatherPreset = weatherPresets[i];

				setter.blockMesh.materials[0].SetColor("_Color", weatherPresets[i].blockTint);
				setter.iconMesh.material.SetTexture("_MainTex", weatherPresets[i].icon);

				setter.tweenHidden.position = weatherHidden.position;
			}
			
			cubes[i] = cube;
			visiblePoints[i] = setter.tweenVisible.gameObject;
		}

		cubeLocationSetter.SetCubePositions(visiblePoints, row == MenuRow.upper, cubeLocationSetter.GetHandedness() == Handedness.right);
		return cubes;
	}

	/*only send one preset array, make sure it matches the type provided otherwise everything will break*/
	public void CreateCubes(int maxPerRow, EnvironmentCubeType type, LightingPreset[] moodPresets = null, WeatherPreset[] weatherPresets = null) {

		int numCubesSet;
		int numUpper;
		int numLower;

		if(type == EnvironmentCubeType.mood) {
			numCubesSet = moodPresets.Length;
		} else {
			numCubesSet = weatherPresets.Length;
		}

		if(numCubesSet > maxPerRow) {
			numUpper = maxPerRow;
			numLower = numCubesSet - numUpper;
		} else {
			numUpper = numCubesSet;
			numLower = 0;
		}

		GameObject[] allCubes = new GameObject[numCubesSet];
		GameObject[] upperCubes;
		GameObject[] lowerCubes;

		SubMenu submenu;

		if(type == EnvironmentCubeType.mood) {
			LightingPreset[] upper = new LightingPreset[numUpper];
			LightingPreset[] lower = new LightingPreset[numLower];

			for(int i = 0; i < moodPresets.Length; i++) {
				if(i < upper.Length) {
					upper[i] = moodPresets[i];
				} else {
					lower[i-upper.Length] = moodPresets[i];
				}
			}

			upperCubes = CreateCubeRow(MenuRow.upper, EnvironmentCubeType.mood, moodPresets: upper);
			lowerCubes = CreateCubeRow(MenuRow.lower, EnvironmentCubeType.mood, moodPresets: lower);

			submenu = moodSubmenu;
		} else {
			WeatherPreset[] upper = new WeatherPreset[numUpper];
			WeatherPreset[] lower = new WeatherPreset[numLower];

			for(int i = 0; i < weatherPresets.Length; i++) {
				if(i < upper.Length) {
					upper[i] = weatherPresets[i];
				} else {
					lower[i-upper.Length] = weatherPresets[i];
				}
			}
			upperCubes = CreateCubeRow(MenuRow.upper, EnvironmentCubeType.weather, weatherPresets: upper);
			lowerCubes = CreateCubeRow(MenuRow.lower, EnvironmentCubeType.weather, weatherPresets: lower);

			submenu = weatherSubmenu;
		}

		for(int i = 0; i < allCubes.Length; i++) {
			if(i < upperCubes.Length) {
				allCubes[i] = upperCubes[i];
			} else {
				allCubes[i] = lowerCubes[i-upperCubes.Length];
			}
		}

		submenu.cubeTweens = new TransformTweenBehaviour[allCubes.Length];
		for (int i = 0; i < allCubes.Length; i++) {
			submenu.cubeTweens[i] = allCubes[i].transform.Find("Cube Tween").GetComponent<TransformTweenBehaviour>();
		}
		
	}

}
