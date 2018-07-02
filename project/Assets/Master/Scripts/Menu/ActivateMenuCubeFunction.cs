using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class ActivateMenuCubeFunction : MonoBehaviour {

	public bool thrown = false;

	private Rigidbody rbCube;
	private AnchorableBehaviour abCube;
	public Transform anchor;

	public float activeDistance;

	public UnityEvent cubeFunction;


	void Start() {
		rbCube = GetComponent<Rigidbody>();
		abCube = GetComponent<AnchorableBehaviour>();
	}

	void Update() {
		if (thrown && (Mathf.Abs(transform.position.x - anchor.position.x) > activeDistance
			|| Mathf.Abs(transform.position.y - anchor.position.y) > activeDistance
			|| Mathf.Abs(transform.position.z - anchor.position.z) > activeDistance)) {
			
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
		transform.position = anchor.position;
		
		abCube.TryAttach();
	}

}
