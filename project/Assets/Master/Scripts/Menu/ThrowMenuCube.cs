using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

public class ThrowMenuCube : MonoBehaviour {

	public MainMenu menuController;

	private ActivateMenuCubeFunction activator;

	private Rigidbody rb;
	private AnchorableBehaviour ab;
	private InteractionBehaviour ib;

	[HideInInspector]
	public bool thisCubeGrasped = false;
	[HideInInspector]
	public float timeThrown = 0f;

	void Start() {
		activator = GetComponent<ActivateMenuCubeFunction>();
		rb = GetComponent<Rigidbody>();
		ab = GetComponent<AnchorableBehaviour>();
		ib = GetComponent<InteractionBehaviour>();
	}

	void Update() {
		if (ab.isAttached) {
			rb.drag = 5;
			rb.angularDrag = 5;
			if(menuController.cubeInUse && thisCubeGrasped) {
				thisCubeGrasped = false;
				menuController.cubeInUse = false;
			}
		}

		if (menuController.cubeInUse && !thisCubeGrasped) {
			ib.ignoreGrasping = true;
		} else {
			ib.ignoreGrasping = false;
		}
	}

	public void Grasping() {
		menuController.cubeInUse = true;
		thisCubeGrasped = true;
		activator.thrown = false;
	}

	public void ThrowCube() {
		if (!ab.isAttached) {
			timeThrown = Time.time;
			rb.drag = 0;
			rb.angularDrag = 0;
			activator.thrown = true;
		}
	}

}
