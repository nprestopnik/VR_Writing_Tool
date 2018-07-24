/*
Guide to Point
Purpose: make the muse guide you to a given point
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the different possible directions the muse could enter from
public enum EnterFromDirection{
	above,below,left,right
}

public class GuideToPoint : MonoBehaviour {

	public EnterFromDirection entryDirection; //the direction from which the muse will enter

	[Tooltip("There should be four entry points in the following order: above, below, left, right")]
	public Transform[] entryPoints; //the array of entry points
	public Transform startPoint; //the point where the muse will sit in front of your face

	public float speed = 5f;

	public Transform target; //where the muse is going to go next

	//public bool guiding = false; //if the muse is currently travelling somewhere
	//this was used for the old movement system; it has gone out of use but it's still here as comments

	private Transform startPosition; //the muse's start position when it begins guiding

	//lerp stuff from the old movement system
	//private float lerpTime = 1f;
	//private float currentLerpTime = 0f;
	
	void Start () {
		
	}
	
	void FixedUpdate () {

		//lerp the muse towards it's given target
		if (target != null) {
			transform.position = Vector3.Lerp(startPosition.position, target.position, Time.deltaTime * speed);
		}

		// if (guiding) {
		// 	if (transform.position != target.position) {
		// 		currentLerpTime += Time.deltaTime;
		// 		if (currentLerpTime > lerpTime) {
		// 			currentLerpTime = lerpTime;
		// 		}
		// 		float t = currentLerpTime/lerpTime;
		// 		//make lerp movement ease in and out
		// 		t = t*t*t * (t * (6f*t-15f) + 10f);
		// 		transform.position = Vector3.Lerp(startPosition.position, target.position, t);
		// 	} else {
		// 		guiding = false;
		// 		currentLerpTime = 0f;
		// 	}
		// }
	}

	public void GuideTo(Transform targetPoint, Action completedEvent = null) {
		//stop if the muse is being cleared
		// if(MuseManager.instance.clearingMuse) {
		// 	MuseManager.instance.clearingMuse = false;
		// 	return;
		// }	

		//guiding = true; //from old guiding system

		//set the target and the start position and let the muse go
		target = targetPoint;
		startPosition = transform;
		MuseManager.instance.SetEffectsActive(true);
		StartCoroutine(MoveToTarget(completedEvent));
	}

	IEnumerator MoveToTarget(Action completedEvent = null) {
		//wait until the muse has reached its target, then start the completed event
		yield return new WaitUntil(()=> IsAtTarget());
		if (completedEvent != null)
			completedEvent();
	}

	public bool IsAtTarget() {

		//if the muse is close enough to its target, we'll say it's at the target and can stop
		return (Vector3.SqrMagnitude(target.position-transform.position) < 0.05f); 

		// if (guiding) return false;
		// else return true;
	}

	public void EnterMuse(Action completedEvent = null) {
		//clear the muse of whatever else it might be doing and wait for that to finish before it enters
		//MuseManager.instance.clearingMuse = true;
		//StartCoroutine(MuseEntry(completedEvent));

		transform.SetParent(MuseManager.instance.transform);
		transform.position = entryPoints[(int)entryDirection].position;
		GuideTo(startPoint, completedEvent);
	}
	// IEnumerator MuseEntry(Action completedEvent = null) {
	// 	yield return new WaitUntil(()=> !MuseManager.instance.clearingMuse); 
	// 	transform.parent.SetParent(null);
	// 	transform.position = entryPoints[(int)entryDirection].position;
	// 	GuideTo(startPoint, completedEvent);
	// }

	Action storedCompletedEvent;
	public void ExitMuse(Action completedEvent = null) {
		storedCompletedEvent = completedEvent;
		//put the muse back where it came from
		GuideTo(entryPoints[(int)entryDirection], CompletedExit);
	}
	void CompletedExit() {
		//keep the muse out of the way and clear its text before doing the callback
		transform.SetParent(entryPoints[(int)entryDirection]);
		MuseManager.instance.SetEffectsActive(false);
		MuseManager.instance.museText.ClearText();
		if (storedCompletedEvent != null)
			storedCompletedEvent();
	}
}
