using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Purpose: UI Panel that contained and managed save loading and deleting
Depricated as UI panels are no longer used */
public class SavePanel : MonoBehaviour {

	public Save save;

	public Text nameText;

	bool isLoad;

	public void initSave(Save s, bool isLoad) {
		save = s;
		nameText.text = save.name;
		this.isLoad = isLoad;
	}

	public void clickButton() {
		if(isLoad) {
			loadSave();
		} else {
			deleteSave();
		}
		transform.parent.parent.parent.parent.gameObject.SetActive(false);
	}

	public void loadSave() {
		SaveSystem.instance.setCurrentSave(save);
		if(TravelSystem.instance.testSetGoalScene(save.currentRoomIndex))
			TravelSystem.instance.setGoalScene(save.currentRoomIndex);
		else {
			TravelSystem.instance.setGoalScene(1);
		}
	}

	public void deleteSave() {
		SaveSystem.instance.deleteSave(save.path);
		Destroy(gameObject);
	}
}
