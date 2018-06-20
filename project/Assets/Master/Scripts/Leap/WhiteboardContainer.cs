using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardContainer : MonoBehaviour {

	public WhiteboardData data;
	public Whiteboard whiteboard;

	public void loadData(WhiteboardData newData) {
		data = newData;
		//setup whiteboard with old data
		whiteboard.loadData(data);
	}

	void Start() {
		data  = new WhiteboardData();
	}

	void Update() {
		//Update the data
		data.lines = whiteboard.lines.ToArray();
		data.position = whiteboard.transform.root.position;
		data.rotation = whiteboard.transform.root.rotation;
	}
}
