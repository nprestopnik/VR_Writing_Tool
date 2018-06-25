using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocation : MonoBehaviour {

	public GameObject trackedDesk;

	private DeskParked deskParkStatus;

	void Start() {
		deskParkStatus = trackedDesk.GetComponent<DeskParked>();
	}

	void Update() {
		//make the target desk line up with the parked position of the tracked desk
		//if the desk is parked, the target will update based on its tracked position
		bool parked = deskParkStatus.parked;
		if(parked) {
			transform.position = trackedDesk.transform.position;
			transform.rotation = trackedDesk.transform.rotation;
		}
	}

}
