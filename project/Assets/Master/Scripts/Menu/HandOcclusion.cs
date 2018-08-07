using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOcclusion : MonoBehaviour {

	public bool handCovering = false;

	public MenuHandedness hands;

	void OnTriggerEnter(Collider col) {
		//check to make sure the collider is the non-menu holding hand
		if(col.gameObject == hands.otherHandModel.gameObject.transform.GetChild(0).GetChild(0).gameObject) {
			handCovering = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.gameObject == hands.otherHandModel.gameObject.transform.GetChild(0).GetChild(0).gameObject) {
			handCovering = false;
		}
	}

	void Update() {
		//make sure the occlusion collider is following the correct hand (the one with the menu on it)
		transform.position = hands.currentHandModel.gameObject.transform.GetChild(0).GetChild(0).position;

		//if the dominant hand has gone out of view, make sure it says that it isn't covering the other hand
		if(!hands.otherHandModel.gameObject.activeSelf) {
			handCovering = false;
		}

	}
}
