using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDebug : MonoBehaviour {

	public Transform[] toCheck;

	void Update () {
		
		foreach(Transform t in toCheck) {
			Debug.Log(t.gameObject.name + " " + t.position);
		}

	}

}
