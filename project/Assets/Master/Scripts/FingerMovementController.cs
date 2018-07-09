using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerMovementController : MonoBehaviour {

	public PlayerController pc;
	Leap.Unity.ExtendedFingerDetector efd;
	public GameObject pointer;

	public bool isFingers = false;
	public bool isPalm = false;

	void Start () {
		efd = GetComponent<Leap.Unity.ExtendedFingerDetector>();
	}
	
	void FixedUpdate () {
		if(isFingers && isPalm) {
			Vector3 dir = pointer.transform.right;
			if(MenuHandedness.dominantHand == Handedness.left) {
				dir = pointer.transform.right * -1;
			}
			dir.y = 0;
			pc.moveInDirection(dir);
		} else {
			pc.moveInDirection(Vector3.zero);
		}
	}

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
