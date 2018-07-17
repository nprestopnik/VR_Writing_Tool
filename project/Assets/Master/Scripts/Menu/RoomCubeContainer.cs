using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCubeContainer : MonoBehaviour {

	int roomIndex;
	public Room room;

	public Texture[] sceneryIcons;
	public MeshRenderer blockMesh;
	public MeshRenderer iconMesh;

	public void initContainer(Room r, int indx) {
		room = r;
		roomIndex = indx;

		blockMesh.material.color = r.color;
		iconMesh.material.SetTexture("_MainTex", sceneryIcons[r.sceneID]);
	}

	public void loadRoom() {
		TravelSystem.instance.setGoalScene(roomIndex);
		transform.root.gameObject.SetActive(false);

		MuseManager.instance.museText.SetText("Your destination has been loaded!\nFollow me to the hallway!", startRoomGuide);
	}

	void startRoomGuide() {
		MuseManager.instance.museGuide.EnterMuse();
		MuseManager.instance.Pause(4f, ()=> MuseManager.instance.museNavigator.NavigateToPoint(MuseManager.instance.museNavigator.hallwayPoint.position, 
			()=> MuseManager.instance.museText.SetText("Go through the hallway\nto the selected room!", 
			()=> MuseManager.instance.museNavigator.GetToHallway())));
	}
	
}
