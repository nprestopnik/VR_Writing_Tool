using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCubeCameraCollision : MonoBehaviour {

	public Collider cameraCollider;
	private Collider cubeCollider;

	void Start () {
		
		cubeCollider = GetComponent<Collider>();
		Physics.IgnoreCollision(cubeCollider, cameraCollider);

	}
	
	
}
