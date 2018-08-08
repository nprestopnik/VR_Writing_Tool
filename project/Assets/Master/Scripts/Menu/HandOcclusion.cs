/*
Hand Occlusion
Purpose: to handle how the menu behaves when one hand covers the other
this is so the menu doesn't close/flip out if the hand holding it gets covered and can't recognize the gesture
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOcclusion : MonoBehaviour {

	public bool handCovering = false; //whether or not the menu hand is being covered

	//menu control scripts
	public MenuHandedness hands;
	public MainMenu menu;

	SkinnedMeshRenderer handMesh; //the mesh renderer of the hand currently holding the menu
	HandMaterialSwap matSwap; //a script used to switch the hand material between opaque and transparent
	Color clearAlpha; //a version of the hand's material color with zero alpha
	Color solidAlpha; //a version of the hand's material color with full alpha (a = 1)
	float speed = 8; //how fast the color should lerp between states
	bool fadeOut = false; //is the menu fading out right now
	bool fadeIn = false; //is the menu fading in right now

	void OnTriggerEnter(Collider col) {
		//check to make sure the collider is the non-menu holding hand and that the menu is open
		if(menu.isActive && col.gameObject == hands.otherHandModel.gameObject.transform.GetChild(0).GetChild(0).gameObject)
			StartFadeOut();
	}
	void OnTriggerStay(Collider col) {
		//check to make sure the collider is the non-menu holding hand and that the menu is open 
		//also check to make sure it's not doing this if it is already fading out
		if(!fadeOut && menu.isActive && col.gameObject == hands.otherHandModel.gameObject.transform.GetChild(0).GetChild(0).gameObject) {
			StartFadeOut();
		}
	}
	void OnTriggerExit(Collider col) {
		if(menu.isActive && col.gameObject == hands.otherHandModel.gameObject.transform.GetChild(0).GetChild(0).gameObject)
			StartFadeIn();
	}


	void StartFadeOut() {
		handCovering = true;

		//get the material for the hand and make it transparent so it can fade out
		GameObject currHand = hands.currentHandModel.transform.GetChild(1).gameObject;
		handMesh = currHand.GetComponent<SkinnedMeshRenderer>();
		matSwap = currHand.GetComponent<HandMaterialSwap>();
		matSwap.SetTransparent();
		
		//set up the clear alpha color
		clearAlpha = handMesh.material.color;
		clearAlpha.a = 0;

		fadeIn = false;
		fadeOut = true;
	}

	void StartFadeIn() {
		handCovering = false;

		//get the proper hand material and ready it to fade back in
		GameObject currHand = hands.currentHandModel.transform.GetChild(1).gameObject;
		currHand.SetActive(true);
		handMesh = currHand.GetComponent<SkinnedMeshRenderer>();
		matSwap = currHand.GetComponent<HandMaterialSwap>();
		
		//set up the solid alpha color
		solidAlpha = handMesh.material.color;
		solidAlpha.a = 1;

		fadeOut = false;
		fadeIn = true;
	}

	void Update() {

		//make sure the occlusion collider is following the correct hand (the one with the menu on it)
		transform.position = hands.currentHandModel.gameObject.transform.GetChild(0).GetChild(0).position;

		//if the dominant hand has gone out of view or the menu is inactive, make sure it says that it isn't covering the other hand
		if((!hands.otherHandModel.gameObject.activeSelf || !menu.isActive) && handMesh) {
			StartFadeIn();
		}


		if(fadeOut) {
			//lerp the hand material to transparent and make it disappear completely at the end
			handMesh.material.color = Color.Lerp(handMesh.material.color, clearAlpha, speed * Time.deltaTime);
			if(handMesh.material.color.a < 0.02f) {
				handMesh.material.color = clearAlpha;
				handMesh.gameObject.SetActive(false);
				fadeOut = false;
			}
		}

		if(fadeIn) {
			//lerp the hand material back to its original alpha and set it back to opaque at the end
			handMesh.material.color = Color.Lerp(handMesh.material.color, solidAlpha, speed * Time.deltaTime);
			if (handMesh.material.color.a > 0.98f) {
				handMesh.material.color = solidAlpha;
				fadeIn = false;
				matSwap.SetOpaque();
			}
		}
		

	}
}
