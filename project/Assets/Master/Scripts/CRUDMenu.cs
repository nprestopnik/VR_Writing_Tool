using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CRUDMenu : MonoBehaviour {

	public Text textDisplay;
	public InputField saveName;

	public GameObject loadContentPanel;
	public GameObject deleteContentPanel;
	public GameObject savePanelPrefab;

	void Start () {
		// Save[] saves = SaveSystem.instance.listSaves();

		// foreach(Save s in saves) {
		// 	SavePanel p = ((GameObject)Instantiate(savePanelPrefab, contentPanel.transform)).GetComponent<SavePanel>();
		// 	p.initSave(s);
		// }
	}
	
	public void Update() {
		// if(Input.GetKeyDown(KeyCode.P)) {
		// 	createNewSave();
		// }
	}

	public void createLoadPanelContent() {
		foreach(Transform t in loadContentPanel.transform) {
			Destroy(t.gameObject);
		}
		
		Save[] saves = SaveSystem.instance.listSaves();

		foreach(Save s in saves) {
			SavePanel p = ((GameObject)Instantiate(savePanelPrefab, loadContentPanel.transform)).GetComponent<SavePanel>();
			p.initSave(s, true);
		}
	}

	public void createDeletePanelContent() {
		foreach(Transform t in deleteContentPanel.transform) {
			Destroy(t.gameObject);
		}
		
		Save[] saves = SaveSystem.instance.listSaves();

		foreach(Save s in saves) {
			SavePanel p = ((GameObject)Instantiate(savePanelPrefab, deleteContentPanel.transform)).GetComponent<SavePanel>();
			p.initSave(s, false);
		}
	}

	public void createNewSave() {
		string path = Application.persistentDataPath + "/" + saveName.text + ".save";//EditorUtility.SaveFilePanel("Choose a name for the save", Application.persistentDataPath, "project", "save");
		Save newSave = SaveSystem.instance.createNewSave(path);
		SaveSystem.instance.setCurrentSave(newSave);
		if(TravelSystem.instance.testSetGoalScene(newSave.currentRoomIndex))
			TravelSystem.instance.setGoalScene(newSave.currentRoomIndex);
		else {
			TravelSystem.instance.setGoalScene(1);
		}
	}
}
