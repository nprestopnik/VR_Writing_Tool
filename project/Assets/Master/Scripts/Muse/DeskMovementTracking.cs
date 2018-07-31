using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskMovementTracking : MonoBehaviour {

	GameObject deskTracker;

	void Start() {
		deskTracker = DeskManager.instance.deskTracker;
	}

	void Update () {
		if(PlayerController.instance.isMoving) {
			transform.position = deskTracker.transform.position;
		}		
	}
}
