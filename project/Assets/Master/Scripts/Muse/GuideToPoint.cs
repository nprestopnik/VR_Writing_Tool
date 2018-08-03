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

	public float speed = 10f;

	public Transform target; //where the muse is going to go next

	private Transform startPosition; //the muse's start position when it begins guiding

	
	void Start () {
		
	}
	
	void FixedUpdate () {

		//lerp the muse towards it's given target
		if (target != null) {
			transform.position = Vector3.Lerp(startPosition.position, target.position, Time.deltaTime * speed);
		}

	}

	public void GuideTo(Transform targetPoint, Action completedEvent = null) {

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
	}

	public void EnterMuse(Action completedEvent = null) {
		transform.SetParent(MuseManager.instance.transform);
		transform.position = entryPoints[(int)entryDirection].position;
		GuideTo(startPoint, completedEvent);
	}


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
