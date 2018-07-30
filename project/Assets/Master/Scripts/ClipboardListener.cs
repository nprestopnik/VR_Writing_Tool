using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*Purpose: API for listening to clipboard data */
public class ClipboardListener : MonoBehaviour {

	public bool debugPrint = true;

	private string lastCopy;

	public delegate void OnClipboardChange(string newCopy); //Event that is called when the clipboard value changes

	public OnClipboardChange onClipboardChange; 


	void Start () {
		lastCopy = GUIUtility.systemCopyBuffer; //sets to the current clipboard
		StartCoroutine(clipboardListener()); //Starts the listener
		onClipboardChange += onClipboardChangeDebug; //Adds debug to the event delegate
	}

	//Debug event for the delegate
	void onClipboardChangeDebug(string newCopy) {
		if(debugPrint) {
			print(newCopy);
		}
	}

	//Listener
	IEnumerator clipboardListener() {
		for(;;) { //Always
			string newCopy = GUIUtility.systemCopyBuffer;
			if(!newCopy.Equals(lastCopy)) { //If the clipboard has changed
				onClipboardChange(newCopy); //Call the event with the new data
				lastCopy = newCopy; //Update the last known clipboard data
			}
			yield return new WaitForSeconds(0.1f); //Check every tenth of a second
		}
	}
}


