using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour {

	public Save save;

	public Text nameText;

	public void initSave(Save s) {
		save = s;
		nameText.text = save.name;
	}

	public void loadSave() {
		SaveSystem.instance.setCurrentSave(save);
		if(save.currentRoomIndex != Hallway.instance.goalRoomIndex)
			Hallway.instance.setGoalScene(save.currentRoomIndex);
		else {
			Hallway.instance.setGoalScene(1);
		}
		ControllerMenu.instance.loadRooms();
	}

	public void deleteSave() {
		SaveSystem.instance.deleteSave(save.path);
		Destroy(gameObject);
	}
}
