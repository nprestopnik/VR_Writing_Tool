using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardTester : MonoBehaviour {

	public GameObject whiteBoardPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.B)) {
			WhiteboardContainer whiteboard = ((GameObject)Instantiate(whiteBoardPrefab, transform.position, transform.rotation)).GetComponentInChildren<WhiteboardContainer>();
			WhiteboardData data = new WhiteboardData();
			data.position = whiteboard.transform.root.position;
			data.rotation = whiteboard.transform.root.rotation;
			whiteboard.loadData(data);
			SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].addFeature(whiteboard.data);
			SaveSystem.instance.saveCurrentSave();
		}
	}
}
