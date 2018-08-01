using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Data structure for saving features. Should be extended for more specific features (See WhiteboardData.cs) */
[System.Serializable]
public class Feature {

	public string name; //Useless
	public string ID; //Useless

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
