using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnterFromDirection{
	above,below,left,right
}

public class GuideToPoint : MonoBehaviour {

	public EnterFromDirection entryDirection;

	public Transform[] entryPoints;
	public Transform startPoint;

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

	public void GuideTo(Transform targetPoint, Action completedEvent = null) {
		guiding = true;
		target = targetPoint;
		startPosition = transform;
		StartCoroutine(MoveToTarget(completedEvent));
	}

	IEnumerator MoveToTarget(Action completedEvent = null) {
		yield return new WaitUntil(()=> IsAtTarget());
		if (completedEvent != null)
		completedEvent();
	}

	public bool IsAtTarget() {
		if (guiding) return false;
		else return true;
	}

	public void EnterMuse(Action completedEvent = null) {
		transform.parent.SetParent(null);
		transform.position = entryPoints[(int)entryDirection].position;
		GuideTo(startPoint, completedEvent);
	}

	Action storedCompletedEvent;
	public void ExitMuse(Action completedEvent = null) {
		storedCompletedEvent = completedEvent;
		GuideTo(entryPoints[(int)entryDirection], CompletedExit);
	}
	void CompletedExit() {
		transform.parent.SetParent(entryPoints[(int)entryDirection]);
		if (storedCompletedEvent != null)
		storedCompletedEvent();
	}
}
