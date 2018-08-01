using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Purpose: Data type for saving the data of whiteboard annotations */
[System.Serializable]
public class LineData {

	[SerializeField]
	public int lMatIndex; //Index corresponding to the material to be used
	public float lineWidth;
	[SerializeField]
	public Vector3[] points; //All the points in the line

	public int sortingOrder;

}
