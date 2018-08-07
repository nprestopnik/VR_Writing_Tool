using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOcclusion : MonoBehaviour {

	public bool handCovering = false;

	public MenuHandedness hands;

	SkinnedMeshRenderer handMesh;
	Color clearAlpha;
	Color solidAlpha;
	float speed = 5;
	bool fadeOut = false;
	bool fadeIn = false;

	void OnTriggerEnter(Collider col) {
		//check to make sure the collider is the non-menu holding hand
		if(col.gameObject == hands.otherHandModel.gameObject.transform.GetChild(0).GetChild(0).gameObject) {
			handCovering = true;
			//hands.currentHandModel.gameObject.SetActive(false);
			handMesh = hands.currentHandModel.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
			
			clearAlpha = handMesh.material.color;
			clearAlpha.a = 0;

			fadeIn = false;
			fadeOut = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.gameObject == hands.otherHandModel.gameObject.transform.GetChild(0).GetChild(0).gameObject) {
			handCovering = false;
			//hands.currentHandModel.gameObject.SetActive(true);
			handMesh = hands.currentHandModel.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
			
			solidAlpha = handMesh.material.color;
			solidAlpha.a = 1;

			fadeOut = false;
			fadeIn = true;
		}
	}

	void Update() {

		if(handMesh) {
			print(handMesh.material.color.a);
		}

		//make sure the occlusion collider is following the correct hand (the one with the menu on it)
		transform.position = hands.currentHandModel.gameObject.transform.GetChild(0).GetChild(0).position;

		//if the dominant hand has gone out of view, make sure it says that it isn't covering the other hand
		if(!hands.otherHandModel.gameObject.activeSelf) {
			handCovering = false;
		}

		if(fadeOut) {
			handMesh.material.color = Color.Lerp(handMesh.material.color, clearAlpha, speed * Time.deltaTime);
			if(handMesh.material.color.a < 0.02f) {
				handMesh.material.color = clearAlpha;
				fadeOut = false;
			}
		}

		if(fadeIn) {
			handMesh.material.color = Color.Lerp(handMesh.material.color, solidAlpha, speed * Time.deltaTime);
			if (handMesh.material.color.a > 0.98f) {
				handMesh.material.color = solidAlpha;
				fadeIn = false;
			}
		}
		

	}
}
