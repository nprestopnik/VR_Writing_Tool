using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDeleteCube : MonoBehaviour {

	RoomCubeContainer rcc;

	void Start () {
		rcc = GetComponent<RoomCubeContainer>();
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if(other.name.Equals("Trash Trigger")) {

			SaveSystem.instance.getCurrentSave().deleteRoom(rcc.roomIndex);
			SaveSystem.instance.saveCurrentSave();

			MainMenu.cubeInUse = false;
			transform.root.gameObject.SetActive(false);
		}
	}
}
