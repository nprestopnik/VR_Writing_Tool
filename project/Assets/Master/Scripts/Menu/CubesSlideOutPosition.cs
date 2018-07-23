/*
Set the location of the cube tween's "hidden" transform according to the hiding spot for that category
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesSlideOutPosition : MonoBehaviour {

	public Transform hidden; //the menu category's hiding spot next to the button

	void Start() {
		transform.position = hidden.position;
	}
	
}
