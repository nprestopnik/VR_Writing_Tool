using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TryingJsonThings : MonoBehaviour {

	private string path;
	private string jsonString;

	// Use this for initialization
	void Start () {
		path = Application.streamingAssetsPath + "/Try.json";
		jsonString = File.ReadAllText(path); //read all text opens, reads, and closes file

		//Debug.Log (jsonString);

		Try TestThings = JsonUtility.FromJson<Try>(jsonString);
		Debug.Log (TestThings.Name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}



[System.Serializable]
public class Try {

	public string Name;
	public int Number;
	public int[] MoreNumbers;

}
