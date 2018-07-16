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

	public Collider cameraCollider;

	[Header("Mood")]
	public GameObject moodCubePrefab;
	public Transform moodHiddenUpper;
	public Transform moodHiddenLower;
	public SubMenu moodSubmenu;

	[Header("Weather")]
	public GameObject weatherCubePrefab;
	public Transform weatherHiddenUpper;
	public Transform weatherHiddenLower;
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

		Vector3 hiddenUpperPosition;
		Vector3 hiddenLowerPosition;

		//set row size, parents, and positioning based on type of row
		if(type == EnvironmentCubeType.mood) {
			rowSize = moodPresets.Length;
			cubes = new GameObject[rowSize];

			prefab = moodCubePrefab;
			upperParent = cubeLocationSetter.moodUpperParent;
			lowerParent = cubeLocationSetter.moodLowerParent;

			hiddenUpperPosition = moodHiddenUpper.position;
			hiddenLowerPosition = moodHiddenLower.position;
		} else {
			rowSize = weatherPresets.Length;
			cubes = new GameObject[rowSize];

			prefab = weatherCubePrefab;
			upperParent = cubeLocationSetter.weatherUpperParent;
			lowerParent = cubeLocationSetter.weatherLowerParent;

			hiddenUpperPosition = weatherHiddenUpper.position;
			hiddenLowerPosition = weatherHiddenLower.position;
		}

		//this is the array that the menu handedness script will use to set cube positions
		GameObject[] visiblePoints = new GameObject[cubes.Length];
		
		for(int i = 0; i < rowSize; i++) {
			
			//make a cube!
			GameObject cube = (GameObject)Instantiate(prefab);

			//this should be the child with all the interaction stuff on it
			GameObject menuCube = cube.transform.Find("Menu Cube").gameObject;

			//make sure cube doesn't collide with camera
			IgnoreCubeCameraCollision collide = menuCube.GetComponent<IgnoreCubeCameraCollision>();
			collide.cameraCollider = cameraCollider;

			WeatherMoodContainer setter = menuCube.GetComponent<WeatherMoodContainer>();

			Vector3 hiddenPosition;

			//put row cubes in correct position and reset transform
			if(row == MenuRow.upper) {
				cube.transform.parent = upperParent.transform;
				hiddenPosition = hiddenUpperPosition;
			} else {
				cube.transform.parent = lowerParent.transform;
				hiddenPosition = hiddenLowerPosition;
			}

			cube.transform.localPosition = Vector3.zero;
			cube.transform.localRotation = Quaternion.identity;

			//take proper preset and set cube materials to look like preset
			if(type == EnvironmentCubeType.mood) {
				cube.name = moodPresets[i].settingName;

				setter.moodPreset = moodPresets[i];
				
				setter.blockMesh.materials[0].SetColor("_Color", moodPresets[i].blockTint);
				setter.iconMesh.material.SetTexture("_MainTex", moodPresets[i].icon);

				setter.tweenHidden.position = hiddenPosition;
				setter.tweenHidden.localPosition -= new Vector3(0,0.05f,0);
			} else {
				cube.name = weatherPresets[i].settingName;

				setter.weatherPreset = weatherPresets[i];

				setter.blockMesh.materials[0].SetColor("_Color", weatherPresets[i].blockTint);
				setter.iconMesh.material.SetTexture("_MainTex", weatherPresets[i].icon);

				setter.tweenHidden.position = hiddenPosition;
				setter.tweenHidden.localPosition -= new Vector3(0,0.05f,0);
			}
			
			//add cubes and tweens to master arrays
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

		//make sure you grab the right preset array - only one should be not null
		if(type == EnvironmentCubeType.mood) {
			numCubesSet = moodPresets.Length;
		} else {
			numCubesSet = weatherPresets.Length;
		}

		//figure out how many cubes will be in each row
		//if the number of cubes is greater than the max per row times 2, the extras will come off the bottom
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

		//split up the presets into an upper and lower row and make the rows
		if(type == EnvironmentCubeType.mood) {
			LightingPreset[] upper = new LightingPreset[numUpper];
			LightingPreset[] lower = new LightingPreset[numLower];

			//put the presets in their rows - fill top then put on bottom
			for(int i = 0; i < moodPresets.Length; i++) {
				if(i < upper.Length) {
					upper[i] = moodPresets[i];
				} else {
					lower[i-upper.Length] = moodPresets[i];
				}
			}

			//create cubes!
			upperCubes = CreateCubeRow(MenuRow.upper, EnvironmentCubeType.mood, moodPresets: upper);
			lowerCubes = CreateCubeRow(MenuRow.lower, EnvironmentCubeType.mood, moodPresets: lower);

			submenu = moodSubmenu;
		} else {
			//same as above but with weather preset types

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

		//make an array with both rows of cubes
		for(int i = 0; i < allCubes.Length; i++) {
			if(i < upperCubes.Length) {
				allCubes[i] = upperCubes[i];
			} else {
				allCubes[i] = lowerCubes[i-upperCubes.Length];
			}
		}

		//add the cube tweens to the submenu so they activate/deactivate properly
		submenu.cubeTweens = new TransformTweenBehaviour[allCubes.Length];
		for (int i = 0; i < allCubes.Length; i++) {
			submenu.cubeTweens[i] = allCubes[i].transform.Find("Cube Tween").GetComponent<TransformTweenBehaviour>();
		}
		
	}

}
