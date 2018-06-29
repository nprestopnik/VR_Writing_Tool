using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesSlideOutPosition : MonoBehaviour {

	public Transform originButton;

	void Update () {
		
		transform.position = originButton.position;

	}
	
}
