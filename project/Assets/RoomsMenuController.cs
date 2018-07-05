using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsMenuController : MonoBehaviour {

	public GameObject roomCubePrefab;
	public GameObject startPoint; 
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
		for(int i = 0; i < rooms.Length; i++) {
			RoomCubeContainer rcc = ((GameObject)Instantiate(roomCubePrefab, startPoint.transform)).GetComponentInChildren<RoomCubeContainer>();
			rcc.transform.parent.localPosition = new Vector3(-0.245f * i, 0, 0);
			rcc.roomIndex = i;
			rcc.room = rooms[i];
		}
	}
}
