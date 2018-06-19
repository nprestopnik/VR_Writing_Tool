using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardFeedback : MonoBehaviour {

	IDictionary<KeyCode, GameObject> keyboard = new Dictionary<KeyCode, GameObject>(); //Holds a key (the keyboard key name as a string, based on the Unity key input names) and value (the corrsponding keyboard key GameObject)

	public Material[] solidArray = new Material[2]; //An array holding the two materials used for each key when not pressed

	public Material[] hoverArray = new Material[2]; //An array holding the two materials used for each key when pressed

	private Material capsLockMat; //Will hold the material for the caps lock indicator lamp

	private bool capsLockOn = false; //A bool to flag if caps lock is currently on or off

	// Use this for initialization
	void Start () {
	
		foreach (Transform child in transform) //In the keyboard transform, loop through every key
 		{     		
			if (child.name != "caps_lock_indicator_light" && child.name != "Fn") { //Ignore the caps lock indicator lamp and the Fn keys. Neither has a valid keyCode
				
				KeyCode thisKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), child.name.ToString()); //Turn the child name into a valid keyCode (all keys in the keyboard [prefab are named by proper keyCode)
				keyboard.Add(thisKeyCode, child.gameObject); //Append the key name and keyboard key GameObject to the dictionary
			
			}

			if (child.name == "caps_lock_indicator_light") { //If this is the caps lock lamp specifically...

				capsLockMat = child.gameObject.GetComponent<Renderer>().material; //Assign the caps lock lamp material for later use
				capsLockMat.SetColor ("_Color", Color.black); //Set the initial color to black (off)

			}
 		} 	
	}
	
	// Update is called once per frame
	void Update () {

		foreach (KeyCode key in keyboard.Keys) { //Loop the keyboard dictionary
     		
			 if (Input.GetKeyDown(key)) { //Listen for keyDown on each key in the dictionary
				keyboard[key].GetComponent<Renderer>().materials = hoverArray; //If down, flip material array to hover
			}
			
			if (Input.GetKeyUp(key)) { //Listen for keuUp on each key in the dictionary
				keyboard[key].GetComponent<Renderer>().materials = solidArray; //If up, flip material array to solid
			}
 		}

		if (Input.GetKeyUp(KeyCode.CapsLock)) { //Listen specifically for KeyUp on caps lock

            capsLockOn = !capsLockOn; //Any time the caps lock key is released, NOT its value
		
			if (capsLockOn) { //If caps lock is on...
				capsLockMat.SetColor ("_Color", Color.green); //Make the material color green
			} else { //If caps lock is off...
				capsLockMat.SetColor ("_Color", Color.black); //Make the material color black
			}
        
		}
	}
}