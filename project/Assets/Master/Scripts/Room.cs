using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room {

	public string name;
	public int sceneID;

	public Room(string name, int sceneID) {
		this.name = name;
		this.sceneID = sceneID;
	}
}
