using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWithControllers : MonoBehaviour {

	public MainMenu menu;
	public GameObject movement;
	
	void Start () {
		menu.ActivateMenu();
		movement.SetActive(true);
	}
	

	void Update () {
		
	}
}
