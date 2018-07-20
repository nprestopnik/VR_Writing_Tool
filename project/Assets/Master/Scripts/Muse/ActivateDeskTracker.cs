/*
Activate Desk Tracker
Purpose: when using less than two controllers with the tracker (or just one? i don't remember) 
	the desk tracker has to be set active manually in start otherwise it will be inactive/not trackings
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeskTracker : MonoBehaviour {

	void Start () {
		DeskManager.instance.deskTracker.SetActive(true);
	}
	
	
}
