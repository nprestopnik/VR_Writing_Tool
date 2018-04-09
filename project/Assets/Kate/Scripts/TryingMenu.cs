using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TryingMenu : MonoBehaviour {

	public Transform menuCanvas;
	public Button buttonPrefab;

	private string path;
	private string jsonString;

	// Use this for initialization
	void Start () {
		path = Application.streamingAssetsPath + "/TryMenu.json";
		jsonString = File.ReadAllText(path); //read all text opens, reads, and closes file

		TryMenu Test = JsonUtility.FromJson<TryMenu>(jsonString);

		Debug.Log (Test.Buttons);
		//somehow instantiate the right number of buttons as children of the same canvas?? and have them show up in grid formation??
		for (int i = 0; i < Test.Buttons; i++) {
			Button button = (Button)Instantiate (buttonPrefab);
			button.transform.SetParent (menuCanvas.transform);
		}
	}

	// Update is called once per frame
	void Update () {

	}

}



[System.Serializable]
public class TryMenu {

	public int Buttons;

}