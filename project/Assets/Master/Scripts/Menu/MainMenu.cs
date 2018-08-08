/*
Main Menu Controller
Purpose: keeps the hand menu kind of together, keeps track of hand gestures and de/activates menu when appropriate
this could probably be redone slightly to be a singleton instead of having just a few static variables
 */

using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using Leap.Unity.Animation;
using Leap.Unity.Interaction;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public GameObject menu;

	private bool finger; //is the index finger extended
	private bool palm; //is the palm facing upward

	public TransformTweenBehaviour menuButtonTween; //the tweens for the central menu buttons

	public SubMenu[] subMenus; //the sub menu scripts attached to each button

	public bool isActive; //is the menu open

	public GameObject movementController; //the object that controls movement

	[HideInInspector]
	public static MenuHandedness menuHandControl; //how we know if the proper hand is active

	[HideInInspector]
	public static bool cubeInUse; //is any cube being used at the moment
	//this boolean is here for individual cubes to reference - its how they know whether some other cube is being used at the moment

	public HandOcclusion occlusionDetector;

	public float lerpSpeed;
	[Tooltip("how far away from the menu your finger can get before the menu lerps back to it")]
	public float maxFreezeDistance;
	[Tooltip("if the menu gets within this distance from the finger, it will stop lerping")]
	public float closeEnoughDistance;
	[Tooltip("how far off of the menu's rotation your finger can get before the menu lerps back to it")]
	public float maxFreezeAngle;
	[Tooltip("if the menu gets within this angle from the finger's rotation, it will stop lerping")]
	public float closeEnoughAngle;
	Transform menuParent; //the parent of the menu on the attachment hands
	bool menuHandVisible = true; //has the menu holding hand gone out of view?
	bool startLerp = true; //should the menu be lerping towards the attachment hand point

	bool doNotDeactivate = false; //stop deactivation under certain circumstances

	void Start() {
		menuHandControl = GetComponent<MenuHandedness>();
		menuParent = menu.transform.parent;
	}

	void Update() {

		//if the hand was not visible and is visible again, put the menu back at its position off the finger
		if(isActive && !menuHandVisible && menuHandControl.handActive) {

			//force finger to be true when the hand becomes visible again
			finger = true; //not a great fix but it kind of works for the reappearing blip

			menuHandVisible = true;
			doNotDeactivate = true;
			startLerp = true;
		}

		//if the menu is trying to deactivate because the hands are no longer visible, stop it from deactivating
		//only actually deactivate menu if the hand gesture is recognized as not being correct anymore
		if(!menuHandControl.handActive) {
			menuHandVisible = false;
			doNotDeactivate = true;
		}

		//if the menu hand is being covered by the other hand, do not deactivate it
		if(occlusionDetector.handCovering) {
			doNotDeactivate = true;
		}

		//if the finger and palm are in the right place, activate the menu (if it isn't open already)
		if(finger && palm) {
			if (!isActive) {
				ActivateMenu();
			}	
 		} else {
			if(isActive) {
				DeactivateMenu();
			}
		}

		//don't let the user accidentally move while trying to throw a menu cube
		if(cubeInUse) {
			movementController.SetActive(false);
		} else {
			movementController.SetActive(true);
		}

		//it's probably really inefficient to calculate these every frame but here they are anyway
		float menuMoved = Vector3.Distance(menu.transform.position, menuParent.position);
		float menuRotated = Quaternion.Angle(menu.transform.rotation, menuParent.rotation);
		//lerp the menu back to the finger if the distance or angle between them gets too great
		//stop moving the menu if it is close enough to the finger/your hand isn't really moving
		if(menuMoved > maxFreezeDistance || menuRotated > maxFreezeAngle) {
			startLerp = true;
		} else if (menuMoved < closeEnoughDistance || menuRotated < closeEnoughAngle) {
			startLerp = false;
		}

		if(occlusionDetector.handCovering) {
			startLerp = false;
		}
		
		//while the menu is unparented from the hand, have it lerp to the position off the finger
		//this is so it will ease into the right position rather than being rigidly locked to the finger
		if(startLerp) {
			menu.transform.position = Vector3.Lerp(menu.transform.position, menuParent.position, Time.deltaTime * lerpSpeed);
			menu.transform.rotation = Quaternion.Lerp(menu.transform.rotation, menuParent.rotation, Time.deltaTime * lerpSpeed);
		}

	}

	public void ActivateMenu() {
		//only allow the menu to be opened if a save has been loaded
		if(SaveSystem.instance.getCurrentSave() != null) {
			isActive = true;

			//animate the buttons in
			menuButtonTween.PlayForward();
			
			//make sure the menu is where it's supposed to be
			ResetMenuTransform();
			//freeze menu in opening location
			menuParent = menu.transform.parent;
			menu.transform.parent = transform;

		}
		
	}

	public void DeactivateMenu() {

		if(doNotDeactivate) {
			doNotDeactivate = false;
			return;
		}

		isActive = false;

		//make sure all submenus are closed along with the menu
		foreach(SubMenu s in subMenus) {
			if (s.subMenuOpen) {
				s.ControlSubMenu();
			}
		}
		
		//close the buttons
		menuButtonTween.PlayBackward();
	
		//unfreeze menu in world
		menu.transform.parent = menuParent;
		ResetMenuTransform();
	}


	//the following are for the finger and palm detectors to set these flags as appropriate
	public void fingerExtend() {
		finger = true;
	}
	public void fingerRetract() {
		finger = false;
	}
	public void palmUp () {
		palm = true;
	}
	public void palmDown() {
		palm = false;
	}

	//resets local position and rotation of menu to all zero
	public void ResetMenuTransform() {
		menu.transform.localPosition = Vector3.zero;
		menu.transform.localRotation = Quaternion.identity;

		//make sure the tween transforms are also at local zero
		foreach(Transform t in menuButtonTween.transform) {
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
		}
	}
}
