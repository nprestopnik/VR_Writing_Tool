using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;


public class ReturnMenuCube : MonoBehaviour {

	private AnchorableBehaviour anchorObj;

	void Start() {
		anchorObj = GetComponent<AnchorableBehaviour>();
	}

	public void ReturnCube() {
		transform.position = anchorObj.anchor.transform.position;
	}
}
