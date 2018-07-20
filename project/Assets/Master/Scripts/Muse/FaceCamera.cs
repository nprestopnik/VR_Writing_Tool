/*
Face Camera
Purpose: make the object the script is attached to always look at the camera
this was used for the muse originally but you could probably tack it on to something else too
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	public Transform cameraHead;

	void Update () {
		transform.LookAt(cameraHead);
	}
}
