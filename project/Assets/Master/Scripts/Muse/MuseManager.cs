using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseManager : MonoBehaviour {

	public static MuseManager instance;

	public MuseText museText;
	public GuideToPoint museGuide;
	public MuseNavigation museNavigator;

	void Awake () {
		instance = this;
	}

	void Start () {
		museGuide.ExitMuse();
	}

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
