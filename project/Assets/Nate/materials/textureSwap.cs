using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureSwap : MonoBehaviour {

	//Make a dictionary for key ID and Mesh
	IDictionary<string, GameObject> keys = new Dictionary<string, GameObject>();
	public Material solidKey;
	public Material hoverKey;

	// Use this for initialization
	void Start () {
		
		foreach (Transform child in transform)
 		{
			//keys.Add(child.name,transform.parent); 
     		
			if (child.name != "caps_lock_indicator_light") { 
				keys.Add(child.name.ToString(), child.gameObject);
				//print(child.gameObject.GetComponent<Renderer>().materials[1]);
			}

			//GetComponent<Renderer>().material.SetColor("_Color", Color.red);
			//print(child.Material);
 		}	

/*
		print(gameObject.name);

		foreach () {

		}

		gameObject.GetComponentInChildren<Material>
		
		
		keys.Add(1,"hello");
		keys.Add(2,"dude");
*/		
		//foreach (KeyValuePair<string, Material> item in keys) {
		//	print(item.Key);
		//	print(item.Value);
		//}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKey("up")) {
			print("up arrow key is held down");
		}
		*/
            
		//NormalGuy.renderer.material.mainTexture = newTexture;
		
	}

	void OnGUI() {
        Event e = Event.current;
        if (e.isKey) {
			//gameObject.GetComponentInChildren<ChildScript>()
            //print("Detected key code: " + e.keyCode);
			string pressed = e.keyCode.ToString();
			GameObject theKey; 
			if (keys.TryGetValue(pressed, out theKey)) {
				print(pressed);
				theKey.GetComponent<Renderer>().materials[1] = hoverKey;
			}
		}
    }
}
