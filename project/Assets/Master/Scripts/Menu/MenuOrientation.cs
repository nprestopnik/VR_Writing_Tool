using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOrientation : MonoBehaviour {

	//public Transform cameraHead;

	void Update () {
		//this.transform.LookAt(cameraHead);
		Vector3 eulers = transform.eulerAngles;
		eulers.z = 0;
		transform.eulerAngles = eulers;
	}
}
