using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handMenuController : MonoBehaviour {

	public GameObject whiteBoardPrefab;
	public DeskManager dm;
	public GameObject RoomsMenu;
	public Transform head;

	void Start () {
		
	}


	public void save() {
		SaveSystem.instance.saveCurrentSave();
	}

	public void quit() {
		Application.Quit();
	}

	public void openRoomLoadMenu() {
		RoomsMenu.SetActive(true);
		RoomsMenu.transform.position = head.forward + head.transform.position;
	}

	public void createIdeaBoard() {
		WhiteboardContainer whiteboard = ((GameObject)Instantiate(whiteBoardPrefab, head.forward + head.transform.position, transform.rotation)).GetComponentInChildren<WhiteboardContainer>();
		WhiteboardData data = new WhiteboardData();
		data.position = whiteboard.transform.root.position;
		data.rotation = whiteboard.transform.root.rotation;
		whiteboard.loadData(data);
		SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].addFeature(whiteboard.data);
		SaveSystem.instance.saveCurrentSave();
	}

	public void activateDesk() {
		dm.DeskTask();
	}
}
