using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Script for cubes that contains room data and updates appearences */
public class RoomCubeContainer : MonoBehaviour {

	public int roomIndex;
	public Room room;

	public MeshRenderer blockMesh; //Color mesh
	public MeshRenderer iconMesh; 

	public void initContainer(Room r, int indx) {
		room = r;
		roomIndex = indx;

		//Updates color and icon of cube
		blockMesh.material.color = r.color;
		iconMesh.material.SetTexture("_MainTex", TravelSystem.instance.sceneryIcons[r.sceneID]);
	}

	public void loadRoom() {
		TravelSystem.instance.setGoalScene(roomIndex); //Loads the room
		transform.root.gameObject.SetActive(false); //Deactivates menus

		//set the muse's text and start its guiding to the hallway
		MuseManager.instance.museNavigator.NavigateToHallway();
	}

	
}
