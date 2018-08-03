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

	private bool finger; //is the index finger extended
	private bool palm; //is the palm facing upward

	public TransformTweenBehaviour[] menuButtonTweens; //the tweens for the central menu buttons

	public SubMenu[] subMenus; //the sub menu scripts attached to each button

	public bool isActive; //is the menu open

	public GameObject movementController; //the object that controls movement

	[HideInInspector]
	public static MenuHandedness menuHandControl;

	[HideInInspector]
	public static bool cubeInUse; //is any cube being used at the moment
	//this boolean is just here for individual cubes to reference - its how they know whether some other cube is being used at the moment

	void Start() {
		menuHandControl = GetComponent<MenuHandedness>();
	}

	void Update() {

		//if the finger and palm are in the right place, activate the menu (if it isn't open already)
		if(finger && palm) {
			if (!isActive) {
				ActivateMenu();
			}	
 		} else {
			 DeactivateMenu();
		}

		if(cubeInUse) {
			movementController.SetActive(false);
		} else {
			movementController.SetActive(true);
		}

	}

	public void ActivateMenu() {
		if(SaveSystem.instance.getCurrentSave() != null) {
			isActive = true;
			//movementController.SetActive(false);

			foreach(TransformTweenBehaviour t in menuButtonTweens) {
				t.PlayForward();
			}
		}
		
		
	}

	public void DeactivateMenu() {

		//if the menu is trying to deactivate because the hands are no longer visible, stop it from deactivating
		//only actually deactivate menu if the hand gesture is recognized as not being correct anymore
		if(!menuHandControl.handActive) {
			return;
		}

		//menu is not active, turn movement back on
		isActive = false;
		//movementController.SetActive(true);

		//make sure all submenus are closed along with the menu
		foreach(SubMenu s in subMenus) {
			if (s.subMenuOpen) {
				s.ControlSubMenu();
			}
		}
		
		//close the buttons
		foreach(TransformTweenBehaviour t in menuButtonTweens) {
			t.PlayBackward();
		}
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
}
