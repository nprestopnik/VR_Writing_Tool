using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class ActivateMenuCubeFunction : MonoBehaviour {

	public bool thrown = false;
	//public MainMenu menuController;

	private Rigidbody rbCube;
	private AnchorableBehaviour abCube;
	private ThrowMenuCube throwCube;
	public Transform anchor;

	//public float activeDistance;
	public float activeSeconds;

	public UnityEvent cubeFunction;


	void Start() {
		rbCube = GetComponent<Rigidbody>();
		abCube = GetComponent<AnchorableBehaviour>();
		throwCube = GetComponent<ThrowMenuCube>();
	}

	void Update() {
		// if (thrown && (Mathf.Abs(transform.position.x - anchor.position.x) > activeDistance
		// 	|| Mathf.Abs(transform.position.y - anchor.position.y) > activeDistance
		// 	|| Mathf.Abs(transform.position.z - anchor.position.z) > activeDistance)) {
		if(thrown && Time.time > throwCube.timeThrown + activeSeconds) {	
			cubeFunction.Invoke();
			thrown = false;
			ReturnCube();
		}
	}

	void OnCollisionEnter() {
		if(thrown) {
			cubeFunction.Invoke();
			thrown = false;
			ReturnCube();
		}
	}

	void ReturnCube() {
		rbCube.velocity = new Vector3(0,0,0);
		rbCube.angularVelocity = new Vector3(0,0,0);
		rbCube.drag = 5;
		rbCube.angularDrag = 5;

		if(throwCube.movingMenu) {
			transform.parent = throwCube.cubeParent;
		}
		if(throwCube.anchorMatch) {
			throwCube.anchorMatch.enabled = true;
			transform.localScale = Vector3.zero;
		}

		transform.position = anchor.position;
		abCube.TryAttach();
		MainMenu.cubeInUse = false;
		throwCube.thisCubeGrasped = false;
	}

}
