using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRawInput;
using System;

public class KeyboardFeedback : MonoBehaviour {

	IDictionary<RawKey, GameObject> keyboard = new Dictionary<RawKey, GameObject>(); //Holds a key (the keyboard key name as a string, based on the Unity key input names) and value (the corrsponding keyboard key GameObject)

	public Material[] solidArray = new Material[2]; //An array holding the two materials used for each key when not pressed

	public Material[] hoverArray = new Material[2]; //An array holding the two materials used for each key when pressed

	private Material capsLockMat; //Will hold the material for the caps lock indicator lamp

	private bool capsLockOn = false; //A bool to flag if caps lock is currently on or off

	// Use this for initialization
	void Start () {
	
			
	}


	//When the desk turns on begin the key checking process
	void OnEnable()
	{
		foreach (Transform child in transform) //In the keyboard transform, loop through every key
 		{     		
			if (child.name != "caps_lock_indicator_light" && child.name != "Fn") { //Ignore the caps lock indicator lamp and the Fn keys. Neither has a valid keyCode
				try {
					RawKey thisKeyCode = (RawKey)Enum.Parse(typeof(RawKey), child.name);
					keyboard.Add(thisKeyCode, child.gameObject); //Append the key name and keyboard key GameObject to the dictionary
				} catch (ArgumentException e) {
					print(e.Message);
				}
				
			
			}

			if (child.name == "caps_lock_indicator_light") { //If this is the caps lock lamp specifically...

				capsLockMat = child.gameObject.GetComponent<Renderer>().material; //Assign the caps lock lamp material for later use
				capsLockMat.SetColor ("_Color", Color.black); //Set the initial color to black (off)

			}
 		}

		RawKeyInput.Start(true);
		RawKeyInput.InterceptMessages = false;
		RawKeyInput.OnKeyDown += HandleKeyDown;
		RawKeyInput.OnKeyUp += HandleKeyUp;
	}

	//When the desk is disabled or the application stops, end the key checking process
	void OnDisable()
	{
		RawKeyInput.Stop();
		RawKeyInput.OnKeyDown -= HandleKeyDown;
		RawKeyInput.OnKeyUp -= HandleKeyUp;
	}
	void OnApplicationQuit()
	{
		RawKeyInput.Stop();
		RawKeyInput.OnKeyDown -= HandleKeyDown;
		RawKeyInput.OnKeyUp -= HandleKeyUp;
	}

	//Event for Key Down
	void HandleKeyDown(RawKey key) {
		if(key == RawKey.CapsLock) {
			capsLockOn = !capsLockOn; //Any time the caps lock key is released, NOT its value
		
			if (capsLockOn) { //If caps lock is on...
				capsLockMat.SetColor ("_Color", Color.green); //Make the material color green
			} else { //If caps lock is off...
				capsLockMat.SetColor ("_Color", Color.black); //Make the material color black
			}
		}
		keyboard[key].GetComponent<Renderer>().materials = hoverArray; //If down, flip material array to hover
	}

	//Event for Key Up
	void HandleKeyUp(RawKey key) {
		keyboard[key].GetComponent<Renderer>().materials = solidArray; //If up, flip material array to solid
	}
	
}