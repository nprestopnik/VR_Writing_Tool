/*
Menu Handedness
Purpose: tracking handedness and setting the position of the menu buttons/cubes correctly based on handedness
Secondary purpose: hurting the heart of anyone who looks at this code because it is the worst
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

//the two options for dominant hand
public enum Handedness {
	left,right
}

public class MenuHandedness : MonoBehaviour {

	[Header("Hand Controls")]
	public static Handedness dominantHand = Handedness.right; //handedness, static so it can be grabbed from elsewhere

	public HandModelBase leftHand; //the leap hand model for the left hand
	public HandModelBase rightHand; //leap hand model for right hand

	//gesture detectors and controller for movement - the hand that has movement has to swap with the menu based on handedness
	public ExtendedFingerDetector movementFingerDetector;
	public PalmDirectionDetector movementPalmDetector;
	public FingerMovementController movementController;

	//the palms (child of the leap hand models) - for changing palm direction detection
	public GameObject leftPalm; 
	public GameObject rightPalm;


	[Header("Menu")]
	public GameObject menu; //the menu itself - the parent of the whole menu
	public GameObject leftHandMenuSpot; //the transform on the left attachment hand where the menu will sit when on that hand
	public GameObject rightHandMenuSpot; //same as above for right hand

	[Header("Buttons")]  //the buttons of the menu!
	public GameObject mood;
	public GameObject creation;
	public GameObject locations;
	public GameObject weather;
	public GameObject system;
	
	public float buttonOffset; //this offset determines the spacing between the buttons


	//these are the possible positions for the menu button. they will be calculated based on the button offset
	//the buttons will be placed in these positions as appropriated
	private Vector3 top;
	private Vector3 bottom;
	private Vector3 left;
	private Vector3 right;
	private Vector3 topLeft;
	private Vector3 topRight;


	[Header("Cubes")]
	[Header("Mood: Set per scene according to environment manager")]
	//grab the set objects that will parent the mood rows 
	//and the objects that represent the transform of where the cubes come out from
	public GameObject moodUpperParent; 
	public GameObject moodUpperHidden;
	public GameObject moodLowerParent;
	public GameObject moodLowerHidden;
	//the actual array of mood cubes will be created and assigned from CreateWeatherMoodCubes
	[HideInInspector]
	public GameObject[] moodCubesUpper;
	[HideInInspector]
	public GameObject[] moodCubesLower;

	//creation, location, and system work the same right now
	//they need the cube parent, the spot where they will hide by the button, and the cubes themselves
	//the cubes that are referenced in the array should be the "visible" transform for the tween
	[Header("Creation")]
	public GameObject creationParent;
	public GameObject creationHidden;
	public GameObject[] creationCubes;

	[Header("Location")]
	public GameObject locationParent;
	public GameObject locationHidden;
	public GameObject[] locationCubes;

	[Header("Weather: Set per scene according to environment manager")]
	//weather is like mood
	public GameObject weatherUpperParent;
	public GameObject weatherUpperHidden;
	public GameObject weatherLowerParent;
	public GameObject weatherLowerHidden;
	[HideInInspector]
	public GameObject[] weatherCubesUpper;
	[HideInInspector]
	public GameObject[] weatherCubesLower;

	[Header("System")]
	public GameObject systemParent;
	public GameObject systemHidden;
	public GameObject[] systemCubes;

	public float cubeOffset; //the spacing between cubes
	public float cubeOffButton; //the spacing between the button and the first button

	//the positions around the menu where button rows can start - to be calculated
	private Vector3 cubeUpperLeft;
	private Vector3 cubeUpperRight;
	private Vector3 cubeTopLeft;
	private Vector3 cubeTopRight;
	private Vector3 cubeMidUpperLeft;
	private Vector3 cubeMidUpperRight;
	private Vector3 cubeMidLowerLeft;
	private Vector3 cubeMidLowerRight;
	private Vector3 cubeBottomLeft;
	private Vector3 cubeBottomRight;

	//the points around the menu where cubes will "hide" when animated
	[HideInInspector]
	public Vector3 hiddenUpperLeft;
	[HideInInspector]
	public Vector3 hiddenUpperRight;
	[HideInInspector]
	public Vector3 hiddenTopLeft;
	[HideInInspector]
	public Vector3 hiddenTopRight;
	[HideInInspector]
	public Vector3 hiddenMidUpperLeft;
	[HideInInspector]
	public Vector3 hiddenMidUpperRight;
	[HideInInspector]
	public Vector3 hiddenMidLowerLeft;
	[HideInInspector]
	public Vector3 hiddenMidLowerRight;
	[HideInInspector]
	public Vector3 hiddenBottomLeft;
	[HideInInspector]
	public Vector3 hiddenBottomRight;


	private MainMenu menuActivator; 
	//the gesture detectors for the menu hand
	private ExtendedFingerDetector fingerDetector;	
	private PalmDirectionDetector palmDetector;

	//the current hand was tracked when you could change handedness from the inspector for testing
	//it isn't really used anymore but it's just chillin here for now
	private Handedness currentHand; 

	[HideInInspector] 
	public bool handActive; //is the hand that holds the menu active - this is for keeping the menu on screen when the hands are not visible

	void Awake() {
		menuActivator = GetComponent<MainMenu>();
		fingerDetector = GetComponent<ExtendedFingerDetector>();
		palmDetector = GetComponent<PalmDirectionDetector>();
		SetPositions();
		SetMenuOrientation();
	}

	void Update() {
		//this was for testing with the public enum; not really used right now
		if(dominantHand != currentHand) {
			SetMenuOrientation();
			currentHand = dominantHand;
		}

		//if the hands go off the screen, set this bool so the menu won't deactivate when you don't look at it
		//the deactivation itself happens in the main menu script
		if(fingerDetector.HandModel.gameObject.activeSelf) {
			handActive = true;
		} else {
			handActive = false;
		}
	}

	//this was a test method for swapping handedness at runtime; it was really only used for preliminary testing with the static handedness varible
	public void swapHands() {
		dominantHand = dominantHand == Handedness.right ? Handedness.left : Handedness.right;
	}

	public Handedness GetHandedness() {
		return dominantHand;
	}

	/*
	Assigns all location variables (for buttons, cubes, hiding spots) to a vector 3 calculated based off of the given offsets
	 */
    void SetPositions() {
		//calculate the visible and hidden positions for the buttons and cubes
		//based on the given offset, with x and z set negative as appropriate

		top = new Vector3(buttonOffset, 0, -buttonOffset);
		bottom = new Vector3(-buttonOffset, 0, buttonOffset);
		left = new Vector3(buttonOffset, 0, buttonOffset);
		right = new Vector3(-buttonOffset, 0, -buttonOffset);
		topLeft = new Vector3(3f*buttonOffset, 0, -buttonOffset);
		topRight = new Vector3(buttonOffset, 0, -3f*buttonOffset);

		float firstCubeOffset = buttonOffset + cubeOffButton;

		cubeUpperLeft = new Vector3(4f*firstCubeOffset, 0, -3f*firstCubeOffset);
		cubeUpperRight = new Vector3(3f*firstCubeOffset, 0, -4f*firstCubeOffset);

		cubeTopLeft = new Vector3(4f*firstCubeOffset, 0, -firstCubeOffset);
		cubeTopRight = new Vector3(firstCubeOffset, 0, -4f*firstCubeOffset);
		cubeMidUpperLeft = new Vector3(4f*firstCubeOffset, 0, firstCubeOffset);
		cubeMidLowerLeft = new Vector3(firstCubeOffset, 0, 4f*firstCubeOffset);
		cubeMidUpperRight = new Vector3(-firstCubeOffset, 0, -4f*firstCubeOffset);
		cubeMidLowerRight = new Vector3(-4f*firstCubeOffset, 0, -firstCubeOffset);
		cubeBottomLeft = new Vector3(-firstCubeOffset, 0, 4f*firstCubeOffset);
		cubeBottomRight = new Vector3(-4f*firstCubeOffset, 0, firstCubeOffset);

		//the hidden locations are only slightly off the cube (less so than the cube positions above)
		hiddenUpperLeft = new Vector3(2f*firstCubeOffset, 0, -2.75f*firstCubeOffset);
		hiddenUpperRight = new Vector3(2.75f*firstCubeOffset, 0, -2f*firstCubeOffset);

		hiddenTopLeft = new Vector3(2f*firstCubeOffset, 0, -firstCubeOffset);
		hiddenTopRight = new Vector3(firstCubeOffset, 0, -2f*firstCubeOffset);
		hiddenMidUpperLeft = new Vector3(2f*firstCubeOffset, 0, firstCubeOffset);
		hiddenMidLowerLeft = new Vector3(firstCubeOffset, 0, 2f*firstCubeOffset);
		hiddenMidUpperRight = new Vector3(-firstCubeOffset, 0, -2f*firstCubeOffset);
		hiddenMidLowerRight = new Vector3(-2f*firstCubeOffset, 0, -firstCubeOffset);
		hiddenBottomLeft = new Vector3(-firstCubeOffset, 0, 2f*firstCubeOffset);
		hiddenBottomRight = new Vector3(-2f*firstCubeOffset, 0, firstCubeOffset);
	}
	

	/*
	put the actual buttons and cubes and whatnot in the correct spot based on the handedness 
		and the desired relative positions of different buttons, etc.
	 */
	void SetMenuOrientation() {
		//the top and bottom buttons don't change based on handedness
		mood.transform.localPosition = top;
		locations.transform.localPosition = bottom;
		if (dominantHand == Handedness.left) {
			//make sure movement is detected on the correct hand (not menu hand)
			movementFingerDetector.HandModel = leftHand;
			movementPalmDetector.HandModel = leftHand;
			movementController.pointer = leftPalm;

			//make sure menu is positioned off non-dominant hand
			fingerDetector.HandModel = rightHand;
			palmDetector.HandModel = rightHand;
			menu.transform.parent = rightHandMenuSpot.transform;
			menu.transform.localPosition = Vector3.zero;
			menu.transform.localRotation = Quaternion.identity;

			//set base buttons in correct position
			weather.transform.localPosition = right;
			creation.transform.localPosition = left;
			system.transform.localPosition = topRight;

			//positin the start of the mood rows correctly
			//the cubes will be created and set from the create cubes script
			moodUpperParent.transform.localPosition = cubeTopLeft;
			moodUpperHidden.transform.localPosition = hiddenTopLeft;
			//SetCubePositions(moodCubesUpper, true, false);
			moodLowerParent.transform.localPosition = cubeMidLowerLeft;
			moodLowerHidden.transform.localPosition = hiddenMidLowerLeft;
			//SetCubePositions(moodCubesLower, false, false);

			//position the other creation and location cubes correctly
			creationParent.transform.localPosition = cubeMidUpperLeft;
			creationHidden.transform.localPosition = hiddenMidUpperLeft;
			SetCubePositions(creationCubes, true, false);

			locationParent.transform.localPosition = cubeBottomLeft;
			locationHidden.transform.localPosition = hiddenBottomLeft;
			SetCubePositions(locationCubes, false, false);

			//position the weather parents, cubes set from create cubes script
			weatherUpperParent.transform.localPosition = cubeTopLeft;
			weatherUpperHidden.transform.localPosition = hiddenTopLeft;
			//SetCubePositions(weatherCubesUpper, true, false);
			weatherLowerParent.transform.localPosition = cubeBottomLeft;
			weatherLowerHidden.transform.localPosition = hiddenBottomLeft;
			//SetCubePositions(weatherCubesLower, false, false);

			//put system cubes in the right spot
			systemParent.transform.localPosition = cubeUpperLeft;
			systemHidden.transform.localPosition = hiddenUpperLeft;
			SetCubePositions(systemCubes, true, false);
		} 
		else if (dominantHand == Handedness.right) {
			//do the same settings as above but adjusted for the menu on the left hand
			movementFingerDetector.HandModel = rightHand;
			movementPalmDetector.HandModel = rightHand;
			movementController.pointer = rightPalm;

			fingerDetector.HandModel = leftHand;
			palmDetector.HandModel = leftHand;
			menu.transform.parent = leftHandMenuSpot.transform;
			menu.transform.localPosition = Vector3.zero;
			menu.transform.localRotation = Quaternion.identity;

			weather.transform.localPosition = left;
			creation.transform.localPosition = right;
			system.transform.localPosition = topLeft;

			moodUpperParent.transform.localPosition = cubeTopRight;
			moodUpperHidden.transform.localPosition = hiddenTopRight;
			//SetCubePositions(moodCubesUpper, true, true);
			moodLowerParent.transform.localPosition = cubeMidLowerRight;
			moodLowerHidden.transform.localPosition = hiddenMidLowerRight;
			//SetCubePositions(moodCubesLower, false, true);

			creationParent.transform.localPosition = cubeMidUpperRight;
			creationHidden.transform.localPosition = hiddenMidUpperRight;
			SetCubePositions(creationCubes, true, true);

			locationParent.transform.localPosition = cubeBottomRight;
			locationHidden.transform.localPosition = hiddenBottomRight;
			SetCubePositions(locationCubes, false, true);

			weatherUpperParent.transform.localPosition = cubeTopRight;
			weatherUpperHidden.transform.localPosition = hiddenTopRight;
			//SetCubePositions(weatherCubesUpper, true, true);
			weatherLowerParent.transform.localPosition = cubeBottomRight;
			weatherLowerHidden.transform.localPosition = hiddenBottomRight;
			//SetCubePositions(weatherCubesLower, false, true);

			systemParent.transform.localPosition = cubeUpperRight;
			systemHidden.transform.localPosition = hiddenUpperRight;
			SetCubePositions(systemCubes, true, true);
		}
	}

	public void SetCubePositions(GameObject[] cubes, bool upper, bool right) {
		 int i = 0;
		 //set each cube in a diagonal line out from the button in the proper direction 
		 //based on the cube offset and whether or not the row is upper/lower and on the right/left
		 foreach(GameObject cube in cubes) {
			if(upper) {
				if(right) {
					cube.transform.localPosition = new Vector3(0, 0, -i*3*cubeOffset);
				} else {
					cube.transform.localPosition = new Vector3(i*3*cubeOffset, 0, 0);
				}
			} else {
				if (right) {
					cube.transform.localPosition = new Vector3(-i*3*cubeOffset, 0, 0);
				} else {
					cube.transform.localPosition = new Vector3(0, 0, i*3*cubeOffset);
				}
			}
			i++;
		 }
    }

}
