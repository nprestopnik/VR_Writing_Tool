/*
Ignore Cube-Camera Collision
Purpose: make sure the cubes don't collide with the player/camera and make you move when you don't want to
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCubeCameraCollision : MonoBehaviour {

	public Collider cameraCollider; //the capsule for the camera
	public Collider[] buttonColliders; //the colliders for the menu buttons - not the most necessary but they're here anyway
	private Collider currentCollider; //this cube's collider

	void Start () {
		
		//get collider
		currentCollider = GetComponent<Collider>();

		//ignore cube-camera collisions
		Physics.IgnoreCollision(currentCollider, cameraCollider);

		//ignore collisions between the cube and the menu buttons, just for funsies
		foreach(Collider c in buttonColliders) {
			Physics.IgnoreCollision(currentCollider, c); 
		}	
	}
	
	
}
