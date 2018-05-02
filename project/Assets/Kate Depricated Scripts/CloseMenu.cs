using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu : MonoBehaviour {

	public void Close() {

		MenuMovement menuScript = transform.parent.parent.GetComponent<MenuMovement>();

		menuScript.menuOpen = false;
		menuScript.menu.gameObject.SetActive(false);
		//menuScript.otherWindow.gameObject.SetActive(false);

	}
}
