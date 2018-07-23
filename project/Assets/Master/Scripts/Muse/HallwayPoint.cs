/*
Hallway Point
Purpose: to detect when the user has arrived at the hallway during muse navigation
it uses the trigger collider to tell the muse that the user has gotten to the door and gone through it s
	o that the muse knows when it can leave from its navigation task
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayPoint : MonoBehaviour {

	void OnTriggerEnter() {
		MuseManager.instance.museNavigator.arrivedAtHallway = true;
	}

	void OnTriggerExit() {
		MuseManager.instance.museNavigator.arrivedAtHallway = false;
	}
}
