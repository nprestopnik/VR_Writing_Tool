using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseController : MonoBehaviour {

	public GameObject muse;

	public Transform idlePoint; //child of camera head muse points
	public Transform spawnPoint; //child of camera head muse points
	public Transform deskTargetPoint; //desk muse point as child of desk
	public Transform deskInactivePoint; //desk inactive location: child of tracker target attached to camera rig
	public Transform deskTrackerPoint; //the tracked spot on the desk - parking spot for the muse

	private GuideToPoint guide; //the guide script attached to the muse

	//how these buttons work is subject to change based on what the menu ends up being
	//this setup is mostly just for testing
	private GameObject deskButton;
	private GameObject confirmButton;
	private GameObject returnButton;
	private GameObject parkButton;
	private GameObject unparkButton;
	private GameObject replaceButton;
	private GameObject closeButton;

	void Start () {
		guide = muse.GetComponent<GuideToPoint>();

		//buttons for testing
		deskButton = this.transform.Find("Activate").gameObject;
		confirmButton = this.transform.Find("Confirm").gameObject;
		returnButton = this.transform.Find("Return").gameObject;
		parkButton = this.transform.Find("Park").gameObject;
		unparkButton = this.transform.Find("UnPark").gameObject;
		replaceButton = this.transform.Find("Replace").gameObject;
		closeButton = this.transform.Find("Deactivate").gameObject;
	}
	
	void Update () {

		//accomodate slight differences in parking location
		if ((deskTrackerPoint.transform.position.x < deskInactivePoint.position.x+0.01 || deskTrackerPoint.transform.position.x > deskInactivePoint.position.x-0.01) 
			&& (deskTrackerPoint.transform.position.z < deskInactivePoint.position.z+0.01 || deskTrackerPoint.transform.position.z > deskInactivePoint.position.z-0.01)
			&& deskTrackerPoint.parent.gameObject.activeSelf) {
			closeButton.SetActive(true);
		} else {
			closeButton.SetActive(false);
		}
	}

	public void ActivateMuse() {
		muse.SetActive(true);
		guide.activation = true;
		guide.guiding = true;
		guide.target = idlePoint;


		//test buttons
		deskButton.SetActive(false);
		confirmButton.SetActive(true);
		returnButton.SetActive(true);
	}

	public void DeactivateMuse(){
		guide.deactivation = true;
		guide.guiding = true;
		guide.target = spawnPoint;


		//test buttons
		deskButton.SetActive(true);
		confirmButton.SetActive(false);
		returnButton.SetActive(false);
	}

	public void GoToDesk(){
		guide.guiding = true;
		guide.target = deskTargetPoint;

		Transform desk = deskTargetPoint.parent;
		desk.gameObject.SetActive(true);
		muse.transform.SetParent(desk);


		//test buttons
		confirmButton.SetActive(false);
		returnButton.SetActive(false);
		parkButton.SetActive(true);
	}

	public void ParkDesk() {
		
		guide.guiding = true;
		guide.target = deskTrackerPoint;


		//test buttons
		parkButton.SetActive(false);
		unparkButton.SetActive(true);
	}

	public void UnParkDesk() {
		
		guide.guiding = true;
		guide.target = deskTargetPoint;


		//test buttons
		unparkButton.SetActive(false);
		parkButton.SetActive(true);
		replaceButton.SetActive(true);
	}

	public void ReplaceDesk() {
		muse.transform.SetParent(idlePoint.parent.parent);
	
		guide.guiding = true;
		guide.target = deskInactivePoint;
		deskInactivePoint.parent.gameObject.SetActive(true);


		//test buttons
		replaceButton.SetActive(false);
	}

	public void CloseDesk() {
		guide.target = spawnPoint;

		Transform desk = deskTargetPoint.parent;
		desk.gameObject.SetActive(false);
		deskInactivePoint.parent.gameObject.SetActive(false);


		//test buttons
		parkButton.SetActive(false);
		unparkButton.SetActive(false);
		closeButton.SetActive(false);
		deskButton.SetActive(true);
	}
}

