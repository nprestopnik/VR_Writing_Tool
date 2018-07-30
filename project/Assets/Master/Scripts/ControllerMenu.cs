﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*DEPRICATED Purpose: events for the old menu to call */
public class ControllerMenu : MonoBehaviour {

	public static ControllerMenu instance;

	public Text saveNameText;
	public Text goalRoomText;

	public GameObject roomPanelPrefab;
	public GameObject contentPanel;

	void Awake() {
		instance = this;
		SceneManager.sceneLoaded += onSceneLoad;
	}

	public void loadRooms () {
		foreach(Transform t in contentPanel.transform) {
			Destroy(t.gameObject);
		}

		Room[] rooms = SaveSystem.instance.getCurrentSave().getRoomsArray();

		//FIX THIS
		for(int i = 0; i < rooms.Length; i++) {
			if(TravelSystem.instance.testSetGoalScene(i)) {
				RoomPanelHolder p = ((GameObject)Instantiate(roomPanelPrefab, contentPanel.transform)).GetComponent<RoomPanelHolder>();
				p.loadRoom(i);
			}	
		}
	}

	void onSceneLoad(Scene scene, LoadSceneMode sceneMode){
		//loadRooms();
	}
	
	void Update () {
		if(SaveSystem.instance.getCurrentSave() != null) {
			saveNameText.text = SaveSystem.instance.getCurrentSave().name;
			goalRoomText.text = TravelSystem.instance.goalRoom.name;
		}
	}

	public void createRoom() {
		Room newRoom = new Room(randomName(), 2, Color.black);
		SaveSystem.instance.getCurrentSave().addRoom(newRoom);
		loadRooms();
		SaveSystem.instance.saveCurrentSave();
	}

	string[] names = new string[]{"Aloha", "Banana", "Room Land", "Testy McTest", "Bloopy Bloop"};

	string randomName() {
		return names[Random.Range(0, names.Length)];
	}
}
