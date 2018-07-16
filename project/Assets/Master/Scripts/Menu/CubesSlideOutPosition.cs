using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesSlideOutPosition : MonoBehaviour {

	public Transform hidden;

	void Start() {
		transform.position = hidden.position;
	}
	
}
