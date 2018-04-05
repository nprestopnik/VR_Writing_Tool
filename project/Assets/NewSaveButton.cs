using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSaveButton : MonoBehaviour {

	public GameObject contentPanel;
	public GameObject savePanelPrefab;

	public void createNewSave() {
		Save newSave = SaveSystem.instance.createNewSave("NEWTESTSAVE");
		SavePanel p = ((GameObject)Instantiate(savePanelPrefab, contentPanel.transform)).GetComponent<SavePanel>();
		p.initSave(newSave);
	}
}
