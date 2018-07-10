using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	public Transform cameraHead;

	void Update () {
		transform.LookAt(cameraHead);
	}
}
