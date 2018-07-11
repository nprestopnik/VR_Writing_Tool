using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskManager : MonoBehaviour {


	public static DeskManager instance;

	public GameObject muse;
	public GameObject deskTracker; //the actual tracker
	public GameObject deskTrackedPoint; //the tracked point on the desk model
	public GameObject targetTracker; //the parent of the "target" for parking the desk
	public GameObject deskModel; //the visual desk object
	public GameObject deskTarget; //the "target" for desk placement
	public Transform parkMusePoint; //where the muse goes when the desk is parked
	public Transform moveMusePoint; //for the muse to stay with the desk while you are parking
	public GameObject lighthouse1; //the lighthouses - make visible to avoid collisions
	public GameObject lighthouse2;

	private bool isTracking;
	private SteamVR_TrackedObject deskTrackedObject;
	private DeskParked deskParked; //whether or not the desk is parked in its "inactive" location

	void Awake () {
		instance = this;
	}

	void Start() {
		deskParked = deskTracker.GetComponent<DeskParked>();
		deskTrackedObject = deskTracker.GetComponent<SteamVR_TrackedObject>();
	}

	void Update() {
		if(isTracking) {
			deskTrackedPoint.transform.position = deskTracker.transform.position;
			deskTrackedPoint.transform.rotation = deskTracker.transform.rotation;
		}

		// if(Vector3.Distance(deskTrackedPoint.transform.position, deskTracker.transform.position) > 0.01f) {
		// 	deskTrackedPoint.transform.position = deskTracker.transform.position;
		// 	deskTrackedPoint.transform.rotation = deskTracker.transform.rotation;
		// }
	}

	public void toggleDeskLock() {
		if(isTracking) {
			ConfirmSet();
			//ConfirmPark();
		} else {

		}
		isTracking = !isTracking;
		
	}

	public void StartDeskTask() {
		MuseManager.instance.museText.SetText("Follow me to your desk!");
		MuseManager.instance.museGuide.EnterMuse();
		MuseManager.instance.Pause(3f, DeskStage20);
	}
	void DeskStage20() {
		MuseManager.instance.museGuide.GuideTo(moveMusePoint, DeskStage30);
	}
	void DeskStage30() {
		deskModel.SetActive(true);
		isTracking = true;
		deskParked.parked = false;
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		MuseManager.instance.museText.SetText("Put your desk where you want it!");
	}

	public void ConfirmSet() {
		isTracking = false;
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		MuseManager.instance.museText.SetText("Bye for now!");
		MuseManager.instance.Pause(3f,()=> MuseManager.instance.museGuide.ExitMuse());
	}


	// public void StartNavgationTest() {
	// 	MuseManager.instance.museText.SetText("Testing navigation!", NavStage10);
	// }
	// void NavStage10() {
	// 	MuseManager.instance.museGuide.EnterMuse();
	// 	MuseManager.instance.Pause(3f, NavStage20);
	// }
	// void NavStage20() {
	// 	//MuseManager.instance.museNavigator.NavigateToPoint(testNavPoint.transform.position, NavStage30);
	// }
	// void NavStage30() {
	// 	//MuseManager.instance.museGuide.GuideTo(testNavPoint.transform);
	// }


	public void StartParkTask() {
		MuseManager.instance.museText.SetText("Follow me to park your desk!");
		MuseManager.instance.museGuide.EnterMuse();
		MuseManager.instance.Pause(3f, ParkStage20);
	}
	void ParkStage20() {
		MuseManager.instance.museGuide.GuideTo(parkMusePoint, ParkStage30);
	}
	void ParkStage30() {
		isTracking = true;
		deskTarget.SetActive(true);
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		MuseManager.instance.museText.SetText("Park your desk here!");
	}

	public void ConfirmPark() {
		deskParked.parked = true;
		deskModel.SetActive(false);
		deskTarget.SetActive(false);
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		MuseManager.instance.museText.SetText("Bye for now!");
		MuseManager.instance.Pause(3f,()=> MuseManager.instance.museGuide.ExitMuse());
	}
	

}
