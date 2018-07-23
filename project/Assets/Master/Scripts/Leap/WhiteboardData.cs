using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WhiteboardData : Feature {

	[SerializeField]
	public LineData[] lines;//All of the drawn lines in on the board

	public string text;	//Test data in the board (image path or plain text)

	public WhiteboardData() : base() {
		name = "Whiteboard";
		text = "";
		lines = new LineData[0];
		scale = new Vector3(0.5f, 0.5f, 1);
	}

	public WhiteboardData(string text) : base() {
		name = "Whiteboard";
		this.text = text;
		lines = new LineData[0];
		scale = new Vector3(0.5f, 0.5f, 1);
	}
}
