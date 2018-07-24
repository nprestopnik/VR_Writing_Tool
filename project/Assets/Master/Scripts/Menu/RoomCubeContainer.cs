using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCubeContainer : MonoBehaviour {

	public int roomIndex;
	public Room room;

	//public Texture[] sceneryIcons;
	public MeshRenderer blockMesh;
	public MeshRenderer iconMesh;

	public void initContainer(Room r, int indx) {
		room = r;
		roomIndex = indx;

		blockMesh.material.color = r.color;
		iconMesh.material.SetTexture("_MainTex", TravelSystem.instance.sceneryIcons[r.sceneID]);
	}

	public void loadRoom() {
		TravelSystem.instance.setGoalScene(roomIndex);
		transform.root.gameObject.SetActive(false);

		//set the destination cube above the hallway to correspond to the loaded room
		// MuseManager.instance.museNavigator.destBlockMesh.material.color = room.color;
		// MuseManager.instance.museNavigator.destIconMesh.material.SetTexture("_MainTex", iconMesh.material.GetTexture("_MainTex"));

		//set the muse's text and start its guiding to the hallway
		MuseManager.instance.museNavigator.NavigateToHallway();
	}

	
}
