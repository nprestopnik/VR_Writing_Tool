using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanelHolder : MonoBehaviour {

	public Text nameText;
	public Room room;
	public int roomIndex;
	void Start () {
		
	}
	
	void Update () {
		
	}

	public void loadRoom(int  newRoomIndex) {
		roomIndex = newRoomIndex;
		room = SaveSystem.instance.getCurrentSave().getRoomsArray()[roomIndex];
		nameText.text = room.name;
		
	}

	public void roomClick() {
		Hallway.instance.setGoalScene(roomIndex);
		//ControllerMenu.instance.loadRooms();
	}
}
