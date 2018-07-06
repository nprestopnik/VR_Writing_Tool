using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCubeContainer : MonoBehaviour {

	public int roomIndex;
	public Room room;

	public void loadRoom() {
		TravelSystem.instance.setGoalScene(roomIndex);
		transform.root.gameObject.SetActive(false);
	}
}
