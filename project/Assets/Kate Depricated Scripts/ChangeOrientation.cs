using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOrientation : MonoBehaviour {

	public Canvas thisWindow;
	public Canvas otherWindow;

	public Transform orient1;
	public Transform orient2;

	public void SwapOrient() {

		int current = transform.parent.parent.GetComponent<MenuMovement> ().menuOrientation;
		Debug.Log (current);
		if (current == 1) {
			transform.parent.parent.GetComponent<MenuMovement> ().menuOrientation = 2;
			thisWindow.transform.localPosition = orient2.localPosition;
			thisWindow.transform.localRotation = orient2.localRotation;

			otherWindow.transform.localPosition = orient1.localPosition;
			otherWindow.transform.localRotation = orient1.localRotation;
		} else if (current == 2) {
			transform.parent.parent.GetComponent<MenuMovement> ().menuOrientation = 1;
			thisWindow.transform.localPosition = orient1.localPosition;
			thisWindow.transform.localRotation = orient1.localRotation;

			otherWindow.transform.localPosition = orient2.localPosition;
			otherWindow.transform.localRotation = orient2.localRotation;
		}

	}
}
