using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineData : MonoBehaviour {

	[SerializeField]
	public Material lMat;
	public float lineWidth;
	[SerializeField]
	public Vector3[] points;

	public int sortingOrder;

}
