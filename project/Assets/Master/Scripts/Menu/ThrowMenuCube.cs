using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Examples;
using Leap.Unity.Interaction;
using UnityEngine;

public class ThrowMenuCube : MonoBehaviour {

	//public MainMenu menuController;

	public SimpleMatchAnchorScaleAndState anchorMatch;
	public bool movingMenu;

	private ActivateMenuCubeFunction activator;

	private Rigidbody rb;
	private AnchorableBehaviour ab;
	private InteractionBehaviour ib;

	[HideInInspector]
	public bool thisCubeGrasped = false;
	[HideInInspector]
	public float timeThrown = 0f;
	[HideInInspector]
	public Transform cubeParent;

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
			if(MainMenu.cubeInUse && thisCubeGrasped) {
				thisCubeGrasped = false;
				MainMenu.cubeInUse = false;
			}
		}

		if (MainMenu.cubeInUse && !thisCubeGrasped) {
			ib.ignoreGrasping = true;
		} else {
			ib.ignoreGrasping = false;
		}
	}

	public void Grasping() {
		MainMenu.cubeInUse = true;
		thisCubeGrasped = true;
		activator.thrown = false;
	}

	public void ThrowCube() {
		if (!ab.isAttached) {
			timeThrown = Time.time;
			rb.drag = 0;
			rb.angularDrag = 0;

			if(movingMenu) {
				cubeParent = transform.parent;
				transform.parent = null;
			}
			if(anchorMatch) {
				anchorMatch.enabled = false;
			}
	
			activator.thrown = true;
		}
	}

}
