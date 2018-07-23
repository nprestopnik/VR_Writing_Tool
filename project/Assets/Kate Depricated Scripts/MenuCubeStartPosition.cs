using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

public class MenuCubeStartPosition : MonoBehaviour {

	public Transform startPoint;

	void OnEnable() {
		transform.position = startPoint.position;
	}

}
