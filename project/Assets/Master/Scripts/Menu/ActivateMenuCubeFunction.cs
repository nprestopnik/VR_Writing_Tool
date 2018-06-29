using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class ActivateMenuCubeFunction : MonoBehaviour {

	public ParticleSystem testParticles;

	public bool thrown = false;

	public Transform cube;
	private Rigidbody rbCube;
	private AnchorableBehaviour abCube;
	public Transform anchor;

	public float activeDistance;

	public UnityEvent cubeFunction;


	void Start() {
		rbCube = cube.GetComponent<Rigidbody>();
		abCube = cube.GetComponent<AnchorableBehaviour>();
	}

	void Update() {
		if (thrown && (Mathf.Abs(cube.position.x - anchor.position.x) > activeDistance
			|| Mathf.Abs(cube.position.y - anchor.position.y) > activeDistance
			|| Mathf.Abs(cube.position.z - anchor.position.z) > activeDistance)) {
			
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
		cube.position = anchor.position;
		
		abCube.TryAttach();
	}

	public void TestCubeFunction() {
		testParticles.Emit(20);
	}

}
