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

	//public bool clearingMuse; //if the muse is called again while it is in the middle of a task, the muse will clear
		//there are checks in the functions of the above muse components to see if this is ever true
		//if the muse is clearing, those functions will reset this boolean and return so they don't mess up the next task
		//then the muse should be clear to continue on with whatever function it was last called to do

	void Awake () {
		instance = this;
	}

	//get the muse out of the way to begin
	void Start () {
		museGuide.ExitMuse();
	}

	void Update() {
		//Debug.Log("Clearing Muse: " + clearingMuse);
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
	
}
