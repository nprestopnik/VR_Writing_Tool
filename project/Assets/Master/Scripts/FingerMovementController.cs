using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Controls movement gesture and playermovement */
public class FingerMovementController : MonoBehaviour {

	public PlayerController pc; //Player controller for movement
	Leap.Unity.ExtendedFingerDetector efd;
	public GameObject pointer;

	public bool isFingers = false;
	public bool isPalm = false;

	void Start () {
		efd = GetComponent<Leap.Unity.ExtendedFingerDetector>();
	}
	
	void FixedUpdate () {
		if(isFingers && isPalm) { //if fingers are curled and palm is facing upwards
			Vector3 dir = pointer.transform.right;
			if(MenuHandedness.dominantHand == Handedness.left) {
				dir = pointer.transform.right * -1;
			}
			dir.y = 0;
			pc.isMoving = true;
			pc.moveInDirection(dir); //Move the player
		} else {
			pc.moveInDirection(Vector3.zero);
			pc.isMoving = false;
		}
	}

	//Pretty self explanitory
	public void fingerExtend() {
		//print("EXTEND");
		isFingers = true;
	}

	public void fingerRetract() {
		//print("RETRACTS");
		isFingers = false;
	}

	public void palmUp() {
		//print("PALM");
		isPalm  = true;
	}

	public void palmDown() {
		//print("NO PALM");
		isPalm = false;
	}
}
