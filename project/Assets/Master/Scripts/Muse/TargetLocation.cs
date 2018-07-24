/*
(Desk) Target Location
Purpose: keep track of the location where the desk was parked
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocation : MonoBehaviour {

	public GameObject trackedDesk; 


	void Update() {
		//make the target desk line up with the parked position of the tracked desk
		//if the desk is parked, the target will update based on its tracked position
		//if the desk is not parked, the target will no longer move based on the tracker
			//so it will stay in the correct position
		
		if(DeskManager.instance.parked) {
			if(trackedDesk.transform.position.y > -75) {
				transform.position = trackedDesk.transform.position;
				transform.rotation = trackedDesk.transform.rotation;
			}
			
		}
	}

}
