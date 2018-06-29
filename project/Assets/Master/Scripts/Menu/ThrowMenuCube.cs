using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMenuCube : MonoBehaviour {

	public ActivateMenuCubeFunction activator;

	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	public void ThrowCube() {
		rb.drag = 0;
		rb.angularDrag = 0;
		activator.thrown = true;
		Debug.Log("cube thrown");
	}

}
