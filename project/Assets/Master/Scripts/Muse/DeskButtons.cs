/*
a collection of desk button functions - just stick these to their buttons/cubes and they'll do their thing
this one's pretty straight-forward I think
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskButtons : MonoBehaviour {

	public void DeskTaskButton() {
		DeskManager.instance.StartDeskTask(true);
	}

	public void ConfirmDeskSetButton() {
		DeskManager.instance.ConfirmSet();
	}

	public void ParkTaskButton() {
		DeskManager.instance.StartParkTask();
	}

	public void ConfirmDeskParkButton() {
		DeskManager.instance.ConfirmPark();
	}

}
