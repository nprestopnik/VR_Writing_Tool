﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCubeCameraCollision : MonoBehaviour {

	public Collider cameraCollider;
	public Collider[] buttonColliders;
	public Collider[] cubeColliders;
	private Collider currentCollider;

	void Start () {
		
		currentCollider = GetComponent<Collider>();

		Physics.IgnoreCollision(currentCollider, cameraCollider);

		foreach(Collider c in buttonColliders) {
			Physics.IgnoreCollision(currentCollider, c); 
		}

		foreach(Collider c in cubeColliders) {
			Physics.IgnoreCollision(currentCollider, c); 
		}
		
	}
	
	
}
