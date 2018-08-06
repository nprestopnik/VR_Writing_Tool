using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOcclusion : MonoBehaviour {

	public bool handCovering = false;

	void OnTriggerEnter(Collider col) {
		//collide with hands but not camera capsule collider
		if(col.tag == "Player" && col.name != "Capsule") {
			handCovering = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.tag == "Player" && col.name != "Capsule") {
			handCovering = false;
		}
	}
}
