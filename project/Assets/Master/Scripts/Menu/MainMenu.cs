using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using Leap.Unity.Animation;
using Leap.Unity.Interaction;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	private bool finger;
	private bool palm;

	public TransformTweenBehaviour[] menuButtonTweens;

	public SubMenu[] subMenus;

	public bool isActive;

	public GameObject movementController;

	private MenuHandedness menuHandControl;

	[HideInInspector]
	public static bool cubeInUse;

	void Start() {
		menuHandControl = GetComponent<MenuHandedness>();
	}

	void Update() {
		if(finger && palm) {
			if (!isActive) {
				ActivateMenu();
			}	
 		} else {
			 DeactivateMenu();
		 }
	}

	public void ActivateMenu() {
		isActive = true;
		movementController.SetActive(false);

		foreach(TransformTweenBehaviour t in menuButtonTweens) {
			t.PlayForward();
		}
	}

	public void DeactivateMenu() {

		if(!menuHandControl.handActive) {
			return;
		}

		isActive = false;
		movementController.SetActive(true);

		foreach(SubMenu s in subMenus) {
			if (s.subMenuOpen) {
				s.ControlSubMenu();
			}
		}
		
		foreach(TransformTweenBehaviour t in menuButtonTweens) {
			t.PlayBackward();
		}
	}

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
