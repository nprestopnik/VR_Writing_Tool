/*
Create Weather and Mood Cubes
Purpose: to dynamically create the weather and mood cubes for the hand menu based on the current scene's environment manager
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Animation;
using System.Linq;

//to differentiate between the upper and lower rows of the menu cubes because there are two
public enum MenuRow {
	upper,lower
}

[RequireComponent(typeof(MenuHandedness))]
public class CreateWeatherMoodCubes : MonoBehaviour {

	public static CreateWeatherMoodCubes instance;

	public Collider cameraCollider; //the cubes don't want to collide with the player

	[Header("Mood")]
	public GameObject moodCubePrefab; //the cube prefab, specific to type 
	public Transform moodHiddenUpper; //hiding spot for the upper row of cubes - where they animate out from
	public Transform moodHiddenLower; //hidden locatoin for lower row
	public SubMenu moodSubmenu; //the submenu that will show these cubes in the hand menu

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

	/*
	Creates one row of cubes, either upper or lower and either weather or mood
	Caller must determine which row, and the type of cubes being made
	Only sent one preset array, and be certain it is of the type specified by the type variable, otherwise everything will break
	If you specify which array you are sending using the named parameter syntax, you don't have to fill the other in with null
	*/
	public WeatherMoodContainer[] CreateCubeRow(MenuRow row, EnvironmentCubeType type, LightingPreset[] moodPresets = null, WeatherPreset[] weatherPresets = null) {
		
		WeatherMoodContainer[] cubes; //this array will hold the entire row of cubes

		int rowSize; //the number of cubes in the row
		GameObject prefab; //which prefab is being used for this row, based on the type of cube
		GameObject upperParent; //the parent for the upper row, based on cube type
		GameObject lowerParent; //lower row parent, based on cube type

		Vector3 hiddenUpperPosition; //position upper cubes will animate out from
		Vector3 hiddenLowerPosition; //position lower cubes will animate out from

		//set row size, prefab, parents, and positioning based on type of row
		if(type == EnvironmentCubeType.mood) {
			rowSize = moodPresets.Length;
			cubes = new WeatherMoodContainer[rowSize];

			prefab = moodCubePrefab;
			upperParent = cubeLocationSetter.moodUpperParent;
			lowerParent = cubeLocationSetter.moodLowerParent;

			hiddenUpperPosition = moodHiddenUpper.position;
			hiddenLowerPosition = moodHiddenLower.position;
		} else {
			rowSize = weatherPresets.Length;
			cubes = new WeatherMoodContainer[rowSize];

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

			//this script is responsible for getting the visual components of the cube, as well as holding its preset for setting
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

			//take correct preset and set cube materials to look like preset (and attach preset to cube)
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
			cubes[i] = setter;
			visiblePoints[i] = setter.tweenVisible.gameObject;
		}

		//use the menu handedness script to set the cubes in position in the line out from the menu
		cubeLocationSetter.SetCubePositions(visiblePoints, row == MenuRow.upper, cubeLocationSetter.GetHandedness() == Handedness.right);
		return cubes;
	}

	/*
	Creates all cubes for either weather or mood - this will need to be called twice for each scene's environment manager
	Caller must determine the type of cubes being made, either weather or mood but not both at once
	Only sent one preset array, and be certain it is of the type specified by the type variable, otherwise everything will break
	If you specify which array you are sending using the named parameter syntax, you don't have to fill the other in with null
	*/
	public void CreateCubes(int maxPerRow, EnvironmentCubeType type, LightingPreset[] moodPresets = null, WeatherPreset[] weatherPresets = null) {

		int numCubesSet; //the number of cubes that are in the given category for the given scene
		int numUpper; //the number of cubes that will go in the top row
		int numLower; //the number of cubes that will go in the bottom row

		//make sure you grab the right preset array - only one should be not null
		if(type == EnvironmentCubeType.mood) {
			numCubesSet = moodPresets.Length;
		} else {
			numCubesSet = weatherPresets.Length;
		}

		//figure out how many cubes will be in each row
		//the top row will fill up completely with the maximum per row before any are put on the bottom
		//if the number of cubes is greater than the max per row times 2, the extras will come off the bottom
		if(numCubesSet > maxPerRow) {
			numUpper = maxPerRow;
			numLower = numCubesSet - numUpper;
		} else {
			numUpper = numCubesSet;
			numLower = 0;
		}

		WeatherMoodContainer[] allCubes = new WeatherMoodContainer[numCubesSet]; //all of the cubes that will be created
		WeatherMoodContainer[] upperCubes; //only the cubes for the upper row
		WeatherMoodContainer[] lowerCubes; //only the cubes for the lower row

		SubMenu submenu; //the submenu that these cubes will be attached to

		//split up the presets into an upper and lower row and make the rows - make sure its the correct type
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

			cubeLocationSetter.moodCubesUpper = upperCubes.Select(x => x.tweenVisible.gameObject).ToArray();
			cubeLocationSetter.moodCubesLower = lowerCubes.Select(x => x.tweenVisible.gameObject).ToArray();

			//grab the right submenu
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

			cubeLocationSetter.weatherCubesUpper = upperCubes.Select(x => x.tweenVisible.gameObject).ToArray();
			cubeLocationSetter.weatherCubesLower = lowerCubes.Select(x => x.tweenVisible.gameObject).ToArray();

			submenu = weatherSubmenu;
		}



		//fill the all cubes array with the cubes from both rows
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
			//get the tween behaviour from each cube and add it to the submenu array
			submenu.cubeTweens[i] = allCubes[i].tween;
		}
		
	}


}
