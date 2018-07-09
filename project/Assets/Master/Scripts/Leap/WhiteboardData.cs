using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WhiteboardData : Feature {

	[SerializeField]
	public LineData[] lines;
	public string text;

	public WhiteboardData() : base() {
		name = "Whiteboard";
		text = "";
		lines = new LineData[0];
		scale = new Vector3(6, 6, 1);
	}
}
