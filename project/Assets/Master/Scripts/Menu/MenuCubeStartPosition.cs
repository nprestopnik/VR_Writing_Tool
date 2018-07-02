using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

public class MenuCubeStartPosition : MonoBehaviour {

	public Transform startPoint;

	void Awake() {
		transform.position = startPoint.position;
	}

	void OnEnable() {
		//Debug.Log("to start position");
		transform.position = startPoint.position;
	}

}
