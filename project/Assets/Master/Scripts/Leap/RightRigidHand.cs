using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*DEPRICATED, USELESS. REALLY SHOULD BE DELETED */
public class RightRigidHand : MonoBehaviour {

	public static Leap.Unity.RigidHand rightHandInstance;
	void Awake () {
		rightHandInstance = GetComponent<Leap.Unity.RigidHand>();
	}
	
	void Update () {
		
	}
}
