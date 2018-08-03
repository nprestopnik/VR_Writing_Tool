using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Creates the menu for room selection */
public class RoomsMenuController : MonoBehaviour {

	public GameObject roomCubePrefab;
	public GameObject startPoint; 

	public GameObject shelf;


	void OnEnable()
	{
		//Destroys any pre-existing cube
		foreach(Transform t in startPoint.transform) {
			Destroy(t.gameObject);
		}

		Room[] rooms = SaveSystem.instance.getCurrentSave().getRoomsArray(); //Gets rooms from project
		shelf.transform.localScale = new Vector3(0.245f * (rooms.Length-1) + 0.1f, 0.03f, 0.3f); //Scales shelf based on room amount

		startPoint.transform.localPosition = new Vector3(0.245f*(rooms.Length-1)/2f -0.1f, 0.12f, 0); //Positions start based on room amount

		for(int i = 0, offset = 0; i < rooms.Length; i++) {
			if(i != SaveSystem.instance.getCurrentSave().currentRoomIndex) { //Checks for if the room is the same as the currently loaded one
				RoomCubeContainer rcc = ((GameObject)Instantiate(roomCubePrefab, startPoint.transform)).GetComponentInChildren<RoomCubeContainer>(); //Creates the room cube and sets its parent
				rcc.transform.parent.localPosition = new Vector3(-0.245f * (i - offset), 0, 0); //Positions the cube based on its index
				rcc.initContainer(rooms[i], i); //Sets the room cube to have the right room
			}  else {
				offset++; //If the room is the same as the one loaded it will add one to the offset
			}
	
		}
		
	}
}
