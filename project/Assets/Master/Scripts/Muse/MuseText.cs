using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuseText : MonoBehaviour {

	public Text museText;

	public void SetText(string text, Action completedEvent = null) {
		if(MuseManager.instance.clearingMuse) {
			MuseManager.instance.clearingMuse = false;
			return;
		}	

		museText.text = text;
		if (completedEvent != null)
			completedEvent();
	}

	public void ClearText(Action completedEvent = null) {
		if(MuseManager.instance.clearingMuse) {
			MuseManager.instance.clearingMuse = false;
			return;
		}	

		museText.text = "";
		if (completedEvent != null)
			completedEvent();
	}
}
