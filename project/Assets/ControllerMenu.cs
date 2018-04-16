using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMenu : MonoBehaviour {

	public Text saveNameText;

	void Start () {
		
	}
	
	void Update () {
		if(SaveSystem.instance.getCurrentSave() != null) {
			saveNameText.text = SaveSystem.instance.getCurrentSave().name;

		}
	}
}
