using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

public class ThrowMenuCube : MonoBehaviour {

	private ActivateMenuCubeFunction activator;

	private Rigidbody rb;
	private AnchorableBehaviour ab;

	void Start() {
		activator = GetComponent<ActivateMenuCubeFunction>();
		rb = GetComponent<Rigidbody>();
		ab = GetComponent<AnchorableBehaviour>();
	}

	void Update() {
		if (ab.isAttached) {
			rb.drag = 5;
			rb.angularDrag = 5;
		}
	}

	public void Grasping() {
		activator.thrown = false;
	}

	public void ThrowCube() {
		if (!ab.isAttached) {
			rb.drag = 0;
			rb.angularDrag = 0;
			activator.thrown = true;
			Debug.Log("cube thrown");
		}
	}

}
