/*
Muse Manager
Purpose: managing the muse
it basically keeps track of the different parts/functions of the muse and lets you access them all from this instance
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseManager : MonoBehaviour {

	public static MuseManager instance;

	public MuseText museText;
	public GuideToPoint museGuide;
	public MuseNavigation museNavigator;

	public GameObject trail;
	public ParticleSystem particles;


	void Awake () {
		instance = this;
	}

	//get the muse out of the way to begin
	void Start () {
		museGuide.ExitMuse();
	}

	void Update() {
		particles.transform.position = museGuide.transform.position;
	}


	/*
	a general pause function for whenever you need the muse to just sit and wait, usually to let the user read text
	 */
	public void Pause(float seconds, Action completedEvent = null) {
		StartCoroutine(PauseForSeconds(seconds, completedEvent));
	}
	IEnumerator PauseForSeconds(float seconds, Action completedEvent = null) {
		yield return new WaitForSeconds(seconds);
		if(completedEvent != null) {
			completedEvent();
		}
	}

	public void SetEffectsActive(bool active) {
		trail.SetActive(active);
		particles.gameObject.SetActive(active);
		particles.Clear();
	}
	
}
