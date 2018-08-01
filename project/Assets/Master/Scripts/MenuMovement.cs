using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*DEPRICATED Purpose: Moving the menu to be on a controller
SUPER NOT BEING USED ANYMORE */
public class MenuMovement: MonoBehaviour {

	private SteamVR_TrackedController thisController;

	Transform controllerTransform;

	public Canvas menu;

	public bool menuOpen = false;
	public int menuOrientation = 1;

	private List<GameObject> buttons; 

	void Awake () {
		controllerTransform = transform;
		menu.gameObject.SetActive (false);
		thisController = GetComponent<SteamVR_TrackedController> ();

		List<Transform> childTransforms = new List<Transform>();
		buttons = new List<GameObject>();
		foreach (Transform trans in childTransforms) {
			GameObject obj = trans.gameObject;
			if (obj.tag == "Button") {
				buttons.Add(obj);
			}
		}

		thisController.MenuButtonClicked += MenuControl;
			
	}

	void MenuControl(object sender, ClickedEventArgs e) {

		if (menuOpen) {
			menu.gameObject.SetActive (false);
			//otherWindow.gameObject.SetActive(false);
			menuOpen = false;
		} else {
			menu.gameObject.SetActive (true);
			//otherWindow.gameObject.SetActive(true);
			menuOpen = true;
		}

	}


	void Update () {
			
	}




}
