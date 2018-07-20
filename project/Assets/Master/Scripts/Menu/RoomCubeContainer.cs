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

		//set the destination cube above the hallway to correspond to the loaded room
		MuseManager.instance.museNavigator.destBlockMesh.material.color = room.color;
		MuseManager.instance.museNavigator.destIconMesh.material.SetTexture("_MainTex", iconMesh.material.GetTexture("_MainTex"));

		//set the muse's text and start its guiding to the hallway
		MuseManager.instance.museText.SetText("Your destination has been loaded!\nFollow me to the hallway!", startRoomGuide);
	}

	void startRoomGuide() {
		//use the muse callback system to bring the muse in, have it navigate to a point, and wait at the hallway with the right text
		MuseManager.instance.museGuide.EnterMuse(); //this might have been able to muse the callback system through enter muse??? hmmmm....
		MuseManager.instance.Pause(4f, ()=> MuseManager.instance.museNavigator.NavigateToPoint(MuseManager.instance.museNavigator.hallwayPoint.position, 
			()=> MuseManager.instance.museText.SetText("Go through the hallway\nto the selected room!", 
			()=> MuseManager.instance.museNavigator.GetToHallway())));


	}
	
}
