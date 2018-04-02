using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CRUDMenu : MonoBehaviour {

	public Text textDisplay;

	void Start () {
		Save[] saves = SaveSystem.instance.listSaves();
		string text = "";
		foreach(Save s in saves) {
			text += s.name + " | " + s.currentRoomID + " | " + s.path + "\n\n";
		}
		textDisplay.text = text;

		//Open save from browse file menu??
		//string path = EditorUtility.OpenFilePanel("Open a save", Application.persistentDataPath, "save");
	}
	
	void Update () {
		
	}
}
