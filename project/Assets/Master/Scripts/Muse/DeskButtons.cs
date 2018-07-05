using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskButtons : MonoBehaviour {

	public void DeskTaskButton() {
		DeskManager.instance.StartDeskTask();
	}

	public void ConfirmDeskSetButton() {
		DeskManager.instance.ConfirmSet();
	}

	public void ParkTaskButton() {
		DeskManager.instance.ParkTask();
	}

	public void ConfirmDeskParkButton() {
		DeskManager.instance.ConfirmPark();
	}

}
