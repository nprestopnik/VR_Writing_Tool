using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClipboardListener : MonoBehaviour {

	public bool debugPrint = true;

	private string lastCopy;

	public delegate void OnClipboardChange(string newCopy);

	public OnClipboardChange onClipboardChange;


	void Start () {
		lastCopy = GUIUtility.systemCopyBuffer;
		StartCoroutine(clipboardListener());
		onClipboardChange += onClipboardChangeDebug;
	}
	
	void Update () {
		
	}

	void onClipboardChangeDebug(string newCopy) {
		if(debugPrint) {
			print(newCopy);
		}
	}

	IEnumerator clipboardListener() {
		for(;;) {
			string newCopy = GUIUtility.systemCopyBuffer;
			if(!newCopy.Equals(lastCopy)) {
				onClipboardChange(newCopy);
				lastCopy = newCopy;
				//Invoke(OnClipboardChange());
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}


