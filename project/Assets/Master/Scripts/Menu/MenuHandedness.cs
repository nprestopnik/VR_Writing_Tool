using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

public enum Handedness {
	left,right
}

/*this is officially a Terrible Script and I am so sorry to whoever has to fix this menu*/
public class MenuHandedness : MonoBehaviour {

	[Header("Hand Controls")]
	public static Handedness dominantHand = Handedness.right;

	public HandModelBase leftHand;
	public HandModelBase rightHand;

	public ExtendedFingerDetector movementFingerDetector;
	public PalmDirectionDetector movementPalmDetector;
	public FingerMovementController movementController;
	public GameObject leftPalm;
	public GameObject rightPalm;

	[Header("Menu")]
	public GameObject menu;
	public GameObject leftHandMenuSpot;
	public GameObject rightHandMenuSpot;

	[Header("Buttons")] 
	public GameObject mood;
	public GameObject creation;
	public GameObject locations;
	public GameObject weather;
	public GameObject system;
	
	public float buttonOffset;

	private Vector3 top;
	private Vector3 bottom;
	private Vector3 left;
	private Vector3 right;
	private Vector3 topLeft;
	private Vector3 topRight;

	[Header("Cubes")]
	[Header("Mood: Set per scene via environment manager")]
	public GameObject moodUpperParent;
	public GameObject moodUpperHidden;
	public GameObject moodLowerParent;
	public GameObject moodLowerHidden;
	[HideInInspector]
	public GameObject[] moodCubesUpper;
	[HideInInspector]
	public GameObject[] moodCubesLower;

	[Header("Creation")]
	public GameObject creationParent;
	public GameObject creationHidden;
	public GameObject[] creationCubes;

	[Header("Location")]
	public GameObject locationParent;
	public GameObject locationHidden;
	public GameObject[] locationCubes;

	[Header("Weather: Set per scene via environment manager")]
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

	public float cubeOffset;
	public float cubeOffButton;

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
	private ExtendedFingerDetector fingerDetector;
	private PalmDirectionDetector palmDetector;
	private Handedness currentHand;
	private Transform currentParent;

	[HideInInspector] 
	public bool handActive;

	void Awake() {
		menuActivator = GetComponent<MainMenu>();
		fingerDetector = GetComponent<ExtendedFingerDetector>();
		palmDetector = GetComponent<PalmDirectionDetector>();
		SetPositions();
		SetMenuOrientation();
	}

	void Update() {
		if(dominantHand != currentHand) {
			SetMenuOrientation();
			currentHand = dominantHand;
		}

		if(fingerDetector.HandModel.gameObject.activeSelf) {
			handActive = true;
		} else {
			handActive = false;
		}
	}

	public void swapHands() {
		dominantHand = dominantHand == Handedness.right ? Handedness.left : Handedness.right;
	}

	public Handedness GetHandedness() {
		return dominantHand;
	}

    void SetPositions() {
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
	
	void SetMenuOrientation() {
		mood.transform.localPosition = top;
		locations.transform.localPosition = bottom;
		if (dominantHand == Handedness.left) {
			movementFingerDetector.HandModel = leftHand;
			movementPalmDetector.HandModel = leftHand;
			movementController.pointer = leftPalm;

			fingerDetector.HandModel = rightHand;
			palmDetector.HandModel = rightHand;
			menu.transform.parent = rightHandMenuSpot.transform;
			menu.transform.localPosition = Vector3.zero;
			menu.transform.localRotation = Quaternion.identity;

			weather.transform.localPosition = right;
			creation.transform.localPosition = left;
			system.transform.localPosition = topRight;

			moodUpperParent.transform.localPosition = cubeTopLeft;
			moodUpperHidden.transform.localPosition = hiddenTopLeft;
			//SetCubePositions(moodCubesUpper, true, false);
			moodLowerParent.transform.localPosition = cubeMidLowerLeft;
			moodLowerHidden.transform.localPosition = hiddenMidLowerLeft;
			//SetCubePositions(moodCubesLower, false, false);

			creationParent.transform.localPosition = cubeMidUpperLeft;
			creationHidden.transform.localPosition = hiddenMidUpperLeft;
			SetCubePositions(creationCubes, true, false);

			locationParent.transform.localPosition = cubeBottomLeft;
			locationHidden.transform.localPosition = hiddenBottomLeft;
			SetCubePositions(locationCubes, false, false);

			weatherUpperParent.transform.localPosition = cubeTopLeft;
			weatherUpperHidden.transform.localPosition = hiddenTopLeft;
			//SetCubePositions(weatherCubesUpper, true, false);
			weatherLowerParent.transform.localPosition = cubeBottomLeft;
			weatherLowerHidden.transform.localPosition = hiddenBottomLeft;
			//SetCubePositions(weatherCubesLower, false, false);

			systemParent.transform.localPosition = cubeUpperLeft;
			systemHidden.transform.localPosition = hiddenUpperLeft;
			SetCubePositions(systemCubes, true, false);
		} 
		else if (dominantHand == Handedness.right) {
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
