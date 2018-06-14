using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	public Transform camera;

	void Update () {
		transform.LookAt(camera);
	}
}
