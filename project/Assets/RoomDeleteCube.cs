using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Deletes contained room when enters trigger with the name of "Trash Trigger" */
public class RoomDeleteCube : MonoBehaviour {

	RoomCubeContainer rcc;

	void Start () {
		rcc = GetComponent<RoomCubeContainer>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.name.Equals("Trash Trigger")) { //Checks name

			SaveSystem.instance.getCurrentSave().deleteRoom(rcc.roomIndex); //Removes room from project
			SaveSystem.instance.saveCurrentSave(); //Saves current project
			TravelSystem.instance.setGoalScene(0); //Sets the goal scene to the first index of rooms (Could add a check for if the deleted room is the current hallway goal)

			MainMenu.cubeInUse = false;
			transform.root.gameObject.SetActive(false); //Turns off the delete room menu
		}
	}
}
