using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineData {

	[SerializeField]
	public int lMatIndex;
	public float lineWidth;
	[SerializeField]
	public Vector3[] points;

	public int sortingOrder;

}
