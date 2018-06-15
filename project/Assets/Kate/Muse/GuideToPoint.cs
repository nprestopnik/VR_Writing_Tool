using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideToPoint : MonoBehaviour {

	public Transform target;
	public float speed = 5.0f;
	public float pause = 1.0f;
	public bool guiding = false;

	private Vector3 pos;
	
	void Start () {
		
	}
	
	void Update () {

		if (guiding) {
			if (transform.position != target.position) {
				transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
			} else {
				guiding = false;
			}
		}
	}

	public void GuideTo(Transform targetPoint) {
		guiding = true;
		target = targetPoint;
	}

	//let's not rely on this one okay it doesn't work all that well
	public bool IsAtTarget() {
		if (guiding) return false;
		else return true;
	}
}
