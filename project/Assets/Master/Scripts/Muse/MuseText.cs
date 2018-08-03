/*
Muse Text
Purpose: methods for setting the muse's text and clearing it using the callback system that we set up for the muse
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuseText : MonoBehaviour {

	public Text museText;

	//set the muse's text to the given string
	public void SetText(string text, Action completedEvent = null) {	

		museText.text = text;
		if (completedEvent != null)
			completedEvent();
	}

	//set the muse's text to an empty string
	public void ClearText(Action completedEvent = null) {

		museText.text = "";
		if (completedEvent != null)
			completedEvent();
	}
}
