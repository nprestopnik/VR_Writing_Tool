using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseController : MonoBehaviour {

	public GameObject muse;

	public Transform idlePoint;
	public Transform spawnPoint;
	public Transform deskTargetPoint;

	private GuideToPoint guide;

	//how these buttons work is subject to change based on what the menu ends up being
	//this setup is mostly just for testing
	private GameObject confirmButton;
	private GameObject returnButton;

	void Start () {
		guide = muse.GetComponent<GuideToPoint>();
		confirmButton = this.transform.Find("Confirm").gameObject;
		returnButton = this.transform.Find("Return").gameObject;
	}
	
	void Update () {
		//if (muse.transform.position == idlePoint.position) {
		//accommodating for hovering motion:
		if (muse.transform.position.x == idlePoint.position.x && muse.transform.position.z == idlePoint.position.z
			&& (muse.transform.position.y < idlePoint.position.y+0.1 || muse.transform.position.y > idlePoint.position.y-0.1)) {
			
			confirmButton.SetActive(true);
			returnButton.SetActive(true);
		}
	}

	public void ActivateMuse() {
		muse.SetActive(true);
		guide.activation = true;
		guide.guiding = true;
		guide.target = idlePoint;
	}

	public void DeactivateMuse(){
		confirmButton.SetActive(false);
		returnButton.SetActive(false);

		guide.deactivation = true;
		guide.guiding = true;
		guide.target = spawnPoint;
	}

	public void GoToDesk(){
		confirmButton.SetActive(false);
		returnButton.SetActive(false);
		
		guide.guiding = true;
		guide.target = deskTargetPoint;

		Transform desk = deskTargetPoint.parent;
		desk.gameObject.SetActive(true);
		muse.transform.SetParent(desk);
	}

	public void ParkDesk() {
		guide.parked = true;
	}

	public void UnParkDesk() {
		guide.parked = false;
	}
}

