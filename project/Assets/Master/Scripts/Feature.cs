using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Feature {

	public string name;
	public string ID;

	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;

	public Feature() {
		name = "";
		position = Vector3.zero;
		rotation = Quaternion.identity;
		scale = Vector3.one;
	}
	
}
