using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParkButton : MonoBehaviour {

	//for testing only
	private bool performing = false;

	//controller setup for testing
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}
	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	//a task activator script (like this one) and the muse appear script need to be attached to the desk button
	//for each thing that requires a muse, there will be a script for activating the specific task, and the muse appear script
	//this is subject to change though..... ahhhhhh

	public GameObject muse;
	public float pauseTime;
	public Transform deskParkMusePoint; //point for muse to hover at - where desk should be parked when done
	public GameObject deskTracker; //the actual desk that is being used and tracked
	public GameObject deskTargetTracker; //the parent of the "target" for parking the desk
	
	private GameObject deskParkTarget; //the "target" for desk placement
	private GameObject deskModel; //the desk part of the tracker - child of the actual tracked object
	private Transform museCanvas;
	private GuideToPoint museGuide;
	private MuseAppear museActivator;
	private GameObject textToParkLocation; //text to show the user where to put the desk when done
	private GameObject textToFinish; //confirmation that they're done with the desk
	private DeskParked deskParked; //if the desk is parked in its "inactive" location

	void Start() {
		museActivator = GetComponent<MuseAppear>();
		museGuide = muse.GetComponent<GuideToPoint>();
		deskParked = deskTracker.GetComponent<DeskParked>();
		
		deskModel = deskTracker.transform.Find("Desk").gameObject;
		deskParkTarget = deskTargetTracker.transform.Find("Desk").gameObject;
		museCanvas = muse.transform.Find("Canvas");
		textToParkLocation = museCanvas.Find("Park1").gameObject;
		textToFinish = museCanvas.Find("Park2").gameObject;
	}

	void Update () {
		
		if (Controller.GetHairTriggerDown() && !performing) { 
			//this is the stuff that needs to happen when we get an actual desk button
			//bring the muse in front of the user and perform the task
			museActivator.EnterMuse();
			StartCoroutine(PerformParkTask());
		}

	}

	IEnumerator PerformParkTask() {
		
		//for testing only
		performing = true;

		//show text saying to follow the muse and wait
		textToParkLocation.SetActive(true);
		yield return new WaitForSeconds(pauseTime);
		
		//remove text and have muse guide to the target location
		textToParkLocation.SetActive(false);
		museGuide.GuideTo(deskParkMusePoint);
		//yield return new WaitUntil(()=> museGuide.IsAtTarget());
		yield return new WaitForSeconds(museGuide.pause);

		//set the desk target active and show the text waiting for a confirmation of parking
		textToFinish.SetActive(true);
		deskParkTarget.SetActive(true);
		yield return new WaitUntil(()=> Controller.GetHairTriggerDown());
		
		//when confirmed, make desk disappear and set it as parked
		deskParked.parked = true;
		textToFinish.SetActive(false);
		deskModel.SetActive(false);
		deskParkTarget.SetActive(false);
		museActivator.ExitMuse();

		//for testing only
		performing = false;
	}
}