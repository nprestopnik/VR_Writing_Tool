using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using Leap.Unity.Interaction;
using UnityEngine;

public class GrabMenuCube : MonoBehaviour {

	public TransformTweenBehaviour tween;
	public Transform nullTarget;

	private Transform original;
	private Transform parent;


	public void GrabCube() {
		//tween.targetTransform = nullTarget;
		original = transform;
		parent = transform.parent;
		transform.parent = null;
	}

	public void DropCube(){
		//tween.targetTransform = transform;
		transform.parent = parent;
		transform.position = original.position;
		transform.rotation = original.rotation;
		transform.localScale = original.localScale;
	}
	
}
