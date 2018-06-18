using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideToPoint : MonoBehaviour {

	public Transform target;
	public bool guiding = false;

	private Transform startPosition;
	private float lerpTime = 1f;
	private float currentLerpTime = 0f;
	
	void Start () {
		
	}
	
	void FixedUpdate () {

		if (guiding) {
			if (transform.position != target.position) {
				currentLerpTime += Time.deltaTime;
				if (currentLerpTime > lerpTime) {
					currentLerpTime = lerpTime;
				}
				float t = currentLerpTime/lerpTime;
				//make lerp movement ease in and out
				t = t*t*t * (t * (6f*t-15f) + 10f);
				transform.position = Vector3.Lerp(startPosition.position, target.position, t);
			} else {
				guiding = false;
				currentLerpTime = 0f;
			}
		}
	}

	public void GuideTo(Transform targetPoint) {
		guiding = true;
		target = targetPoint;
		startPosition = transform;
	}

	public bool IsAtTarget() {
		if (guiding) return false;
		else return true;
	}
}
