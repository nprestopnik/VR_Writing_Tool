using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeskTracker : MonoBehaviour {

	void Start () {
		DeskManager.instance.deskTracker.SetActive(true);
	}
	
	
}
