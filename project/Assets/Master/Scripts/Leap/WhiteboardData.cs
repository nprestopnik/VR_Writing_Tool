using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WhiteboardData : Feature {

	[SerializeField]
	public LineData[] lines;

	public WhiteboardData() : base() {
		name = "Whiteboard";
		lines = new LineData[0];
	}
}
