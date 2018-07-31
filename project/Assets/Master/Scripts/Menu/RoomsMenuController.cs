using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsMenuController : MonoBehaviour {

	public GameObject roomCubePrefab;
	public GameObject startPoint; 

	public GameObject shelf;


	void Start () {
		
	}

	void Update () {
		
	}

	void OnEnable()
	{
		foreach(Transform t in startPoint.transform) {
			Destroy(t.gameObject);
		}

		Room[] rooms = SaveSystem.instance.getCurrentSave().getRoomsArray();
		shelf.transform.localScale = new Vector3(0.245f * (rooms.Length-1) + 0.1f, 0.03f, 0.3f);

		startPoint.transform.localPosition = new Vector3(0.245f*(rooms.Length-1)/2f -0.1f, 0.12f, 0);

		for(int i = 0, offset = 0; i < rooms.Length; i++) {
			if(i != SaveSystem.instance.getCurrentSave().currentRoomIndex) {
				RoomCubeContainer rcc = ((GameObject)Instantiate(roomCubePrefab, startPoint.transform)).GetComponentInChildren<RoomCubeContainer>();
				rcc.transform.parent.localPosition = new Vector3(-0.245f * (i - offset), 0, 0);
				rcc.initContainer(rooms[i], i);
				// rcc.roomIndex = i;
				// rcc.room = rooms[i];
			}  else {
				offset++;
			}
	
		}
		
	}
}
