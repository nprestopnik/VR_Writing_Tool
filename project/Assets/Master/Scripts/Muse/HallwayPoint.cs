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
