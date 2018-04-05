using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveListPanel : MonoBehaviour {

	public GameObject contentPanel;
	public GameObject savePanelPrefab;

	void Start () {
		Save[] saves = SaveSystem.instance.listSaves();
		foreach(Save s in saves) {
			SavePanel p = ((GameObject)Instantiate(savePanelPrefab, contentPanel.transform)).GetComponent<SavePanel>();
			p.initSave(s);
		}
	}
	

}
