using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	private SteamVR_TrackedController thisController;
	public SteamVR_TrackedController otherController;

	public Transform controllerTransform;

	public GameObject menu;
	public GameObject pointer;

	public bool menuOpen = false;
	private bool otherMenuOpen;
	private bool pointerActive = false;

	private List<GameObject> buttons; 

	void Awake () {
		thisController = GetComponent<SteamVR_TrackedController> ();
		otherMenuOpen = otherController.GetComponent<Menu>().menuOpen;

		List<Transform> childTransforms = new List<Transform>();
		buttons = new List<GameObject>();
		foreach (Transform trans in childTransforms) {
			GameObject obj = trans.gameObject;
			if (obj.tag == "Button") {
				buttons.Add(obj);
			}
		}
	}
	

	void Update () {
		if (thisController.menuPressed && !menuOpen && !otherMenuOpen) {
			GameObject g = (GameObject)Instantiate (menu, controllerTransform.position, controllerTransform.rotation*menu.transform.rotation);
			g.transform.position = controllerTransform.position+menu.transform.position;
			g.transform.SetParent(thisController.transform);
			menuOpen = true;
		}

		otherMenuOpen = otherController.GetComponent<Menu>().menuOpen;
		if (otherMenuOpen && !pointerActive) {
			GameObject g = (GameObject)Instantiate (pointer, controllerTransform.position, controllerTransform.rotation);
			g.transform.position = controllerTransform.position;
			g.transform.SetParent(thisController.transform);
			g.transform.localPosition = pointer.transform.position;
			pointerActive = true;
		}


	}
}
