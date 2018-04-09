using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClickEvents : MonoBehaviour {

	public GameObject menu;

	public GameObject red;
	public GameObject blue;

	public void testRed() {
		Instantiate(red);
	}

	public void testBlue() {
		Instantiate(blue);
	}

	public void testExit() {

		Destroy(menu);

	}

	void Awake() {
		
	}

	void Update() {
		
	}
}
