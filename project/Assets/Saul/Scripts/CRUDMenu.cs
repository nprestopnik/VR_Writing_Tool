using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CRUDMenu : MonoBehaviour {

	public Text textDisplay;

	public GameObject contentPanel;
	public GameObject savePanelPrefab;

	void Start () {
		Save[] saves = SaveSystem.instance.listSaves();

		foreach(Save s in saves) {
			SavePanel p = ((GameObject)Instantiate(savePanelPrefab, contentPanel.transform)).GetComponent<SavePanel>();
			p.initSave(s);
		}
	}
	
	public void Update() {
		if(Input.GetKeyDown(KeyCode.P)) {
			createNewSave();
		}
	}

	public void createNewSave() {
		string path = EditorUtility.SaveFilePanel("Choose a name for the save", Application.persistentDataPath, "project", "save");
		Save newSave = SaveSystem.instance.createNewSave(path);
		SavePanel p = ((GameObject)Instantiate(savePanelPrefab, contentPanel.transform)).GetComponent<SavePanel>();
		p.initSave(newSave);
		//Hallway.instance.setGoalScene(1);
		p.loadSave();
	}
}
