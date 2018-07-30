/*
Desk Manager
Purpose: managing the desk
Clearly I am the best at comments
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DeskState {Disabled, Placing, Enabled, Parking}

public class DeskManager : MonoBehaviour {

	//public GameObject testNavPoint; //some point for testing navigation

	public static DeskManager instance;

	DeskState currentState = DeskState.Disabled;


	public Calibrator calibrator;
	public GameObject muse;
	public GameObject deskTracker; //the actual tracker
	public GameObject deskTrackedPoint; //the tracked point on the desk model
	public GameObject targetTracker; //the parent of the "target" for parking the desk
	public GameObject deskModel; //the visual desk object
	public GameObject deskTarget; //the "target" for desk placement
	public GameObject lockPoint;
	public Transform parkMusePoint; //where the muse goes when the desk is parked
	public Transform moveMusePoint; //for the muse to stay with the desk while you are parking
	public GameObject lighthouse1; //the lighthouses - make visible to avoid collisions
	public GameObject lighthouse2;
	public GameObject desktopDisplay;

	public MeshRenderer deskMount;
	public MeshRenderer[] deskWood;
	public Material deskMountMAT;
	public Material deskWoodMAT;
	public Material ghostlyMountMAT;
	public Material ghostlyWoodMAT;

	public bool parked = true; //whether or not the desk is parked in its "inactive" location

	private bool isTracking; //are we supposed to be tracking the tracker irhgt now
	private SteamVR_TrackedObject deskTrackedObject;
	 

	public Leap.Unity.Interaction.Anchor anchor;
	public Leap.Unity.Interaction.AnchorGroup anchorGroup;

	MeshRenderer[] chairMeshes;

	void Awake () {
		instance = this;
	}

	void Start() {
		deskTrackedObject = deskTracker.GetComponent<SteamVR_TrackedObject>();

		chairMeshes = calibrator.getChairController().GetComponentsInChildren<MeshRenderer>(true);
	}

	void Update() {
		if(currentState == DeskState.Placing) {
			//currentState = DeskState.Enabled;
			isTracking = true;

			if(deskMount.material != ghostlyMountMAT) {
				setDeskMaterials(true);
			}

			// if(desktopDisplay.activeInHierarchy) {
			// 	desktopDisplay.SetActive(false);
			// }
		} else if(currentState == DeskState.Parking) {
			//currentState = DeskState.Disabled;
			isTracking = true;
			if(deskMount.material != ghostlyMountMAT) {
				setDeskMaterials(true);
			}

			// if(desktopDisplay.activeInHierarchy) {
			// 	desktopDisplay.SetActive(false);
			// }
		} else if(currentState == DeskState.Enabled) {
			//currentState = DeskState.Placing;
			isTracking = false;
			if(deskMount.material != deskMountMAT) {
				setDeskMaterials(false);
			}

			// if(!desktopDisplay.activeInHierarchy) {
			// 	desktopDisplay.SetActive(true);
			// }
		}


		if(isTracking) {
			deskTrackedPoint.transform.position = deskTracker.transform.position;
			deskTrackedPoint.transform.rotation = deskTracker.transform.rotation;
		}

		if(Vector3.Distance(lockPoint.transform.position, deskTracker.transform.position) <  0.3f && Quaternion.Angle(lockPoint.transform.rotation, deskTracker.transform.rotation) < 5f && currentState == DeskState.Parking) {
			currentState = DeskState.Disabled;
			ConfirmPark();
		}
	}

	float cooldown;

	public void toggleDeskLock() {
		
		if(cooldown < Time.time) {
			
			if(currentState == DeskState.Placing) {
				currentState = DeskState.Enabled;
				ConfirmSet();
				//isTracking = false;
			} else if(currentState == DeskState.Parking) {
				currentState = DeskState.Enabled;
				ConfirmSet();
				//isTracking = true;
				
			} else if(currentState == DeskState.Enabled) {
				currentState = DeskState.Placing;
				StartDeskTask();
				//isTracking = true;
			}

			cooldown = Time.time + 0.5f;
			
			// if(isTracking) {
			// 	ConfirmSet();
			// 	//ConfirmPark();
			// } else {

			// }
			//isTracking = !isTracking;
		}
		
		
	}

	void setDeskMaterials(bool isGhostly) {
		if(isGhostly) {
			deskMount.material = ghostlyMountMAT;
			foreach(MeshRenderer m in deskWood) {
				m.material = ghostlyWoodMAT;
			}
			foreach(MeshRenderer m in chairMeshes) {
				m.material = ghostlyMountMAT;
			}
		} else {
			deskMount.material = deskMountMAT;
			foreach(MeshRenderer m in deskWood) {
				m.material = deskWoodMAT;
			}
			foreach(MeshRenderer m in chairMeshes) {
				m.material = deskMountMAT;
			}
		}
	}

	public void StartDeskTask() {
		if(deskTracker.transform.position.y < -75) {
			MuseManager.instance.museText.SetText("The tracker is not tracking!\nGo fix it and try again.");
			MuseManager.instance.museGuide.EnterMuse();
			MuseManager.instance.Pause(2f, ()=> MuseManager.instance.museGuide.ExitMuse());
		} else {
			MuseManager.instance.museText.SetText("Follow me to your desk!");
			MuseManager.instance.museGuide.EnterMuse();
			MuseManager.instance.Pause(3f, ()=> MuseManager.instance.museGuide.GuideTo(moveMusePoint, DeskStage30));
		}
		
	}
	// void DeskStage20() {	MuseManager.instance.museGuide.GuideTo(moveMusePoint, DeskStage30); } //simplified into above
	void DeskStage30() {
		deskModel.SetActive(true);
		foreach(Transform t in calibrator.getChairController().transform) {
			t.gameObject.SetActive(true);
		}
		//isTracking = true;
		currentState = DeskState.Placing;
		parked = false;
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		MuseManager.instance.museText.SetText("Put your desk where you want it!");
	}

	//once the desk is locked, the muse will exit
	public void ConfirmSet() {
		//isTracking = false;
		currentState = DeskState.Enabled;
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		deskTarget.SetActive(false);
		MuseManager.instance.museText.SetText("Bye for now!");
		MuseManager.instance.Pause(3f,()=> MuseManager.instance.museGuide.ExitMuse());
	}

	/*
	use callbacks to activate muse, show where the desk is to be parked, do associated activation and all that
	 */
	public void StartParkTask() {
		MuseManager.instance.museText.SetText("Follow me to park your desk!");
		MuseManager.instance.museGuide.EnterMuse();
		MuseManager.instance.Pause(3f, ()=> MuseManager.instance.museGuide.GuideTo(parkMusePoint, ParkStage30));
	}
	// void ParkStage20() { MuseManager.instance.museGuide.GuideTo(parkMusePoint, ParkStage30); } //simplified into above
	void ParkStage30() {
		//isTracking = true;
		currentState = DeskState.Parking;
		deskTarget.SetActive(true);
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		MuseManager.instance.museText.SetText("Park your desk here!");
	}

	//confirm that the desk is parked where it is supposed to be and the muse leaves
	public void ConfirmPark() {
		currentState = DeskState.Disabled;
		parked = true;
		
		foreach(Transform t in calibrator.getChairController().transform) {
			t.gameObject.SetActive(false);
		}

		deskModel.SetActive(false);
		deskTarget.SetActive(false);
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		MuseManager.instance.museText.SetText("Bye for now!");
		MuseManager.instance.Pause(3f,()=> MuseManager.instance.museGuide.ExitMuse());
	}


	//testing the muse navigation system here for some reason

	// public void StartNavgationTest() {
	// 	MuseManager.instance.museText.SetText("Testing navigation!", NavStage10);
	// }
	// void NavStage10() {
	// 	MuseManager.instance.museGuide.EnterMuse();
	// 	MuseManager.instance.Pause(3f, NavStage20);
	// }
	// void NavStage20() {
	// 	MuseManager.instance.museNavigator.NavigateToPoint(testNavPoint.transform.position, NavStage30);
	// }
	// void NavStage30() {
	// 	MuseManager.instance.museGuide.GuideTo(testNavPoint.transform);
	// }
	

}
