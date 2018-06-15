using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeskButton : MonoBehaviour {

	//for testing only
	private bool performing = false;
	
	//controller stuff for testing
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
	public Transform deskParkMusePoint; //where the muse goes when the desk is parked
	public GameObject deskTracker;

	private GameObject deskModel;
	private Transform museCanvas; 
	private GuideToPoint museGuide;
	private MuseAppear museActivator;
	private GameObject textToDesk; //the text (child of the muse canvas) that the muse will take you to the desk
	private GameObject textMoveDesk; //the text that says that you can put the desk where you want
	private DeskParked deskParked; //whether or not the desk is parked in its "inactive" location

	void Start() {
		museActivator = GetComponent<MuseAppear>();
		museGuide = muse.GetComponent<GuideToPoint>();
		deskParked = deskTracker.GetComponent<DeskParked>();

		deskModel = deskTracker.transform.Find("Desk").gameObject;
		museCanvas = muse.transform.Find("Canvas");
		textToDesk = museCanvas.Find("Desk1").gameObject;
		textMoveDesk = museCanvas.Find("Desk2").gameObject;	
	}

	void Update () {
		
		if (Controller.GetHairTriggerDown() && !performing) {
			//this is the stuff that needs to happen when we get an actual desk button
			//bring the muse in front of the user and perform the task
			museActivator.EnterMuse();
			StartCoroutine(PerformDeskTask());
		}

	}

	IEnumerator PerformDeskTask() {
		
		//for testing only
		performing = true;

		//show the text to follow the muse and leave it there for the pause time
		textToDesk.SetActive(true);
		yield return new WaitForSeconds(pauseTime);
		
		//get rid of the first text and send muse to the desk
		textToDesk.SetActive(false);
		museGuide.GuideTo(deskParkMusePoint);
		//yield return new WaitUntil(()=> museGuide.IsAtTarget());
		yield return new WaitForSeconds(museGuide.pause);

		//activate the desk and indicate that it is no longer parked
		//tell user to place desk and wait for confirmation that they're done
		deskModel.SetActive(true);
		deskParked.parked = false;
		textMoveDesk.SetActive(true);
		yield return new WaitUntil(()=> Controller.GetHairTriggerDown());

		//get rid of the text and get the muse to leave
		textMoveDesk.SetActive(false);
		museActivator.ExitMuse();

		//for testing only
		performing = false;
	}
}
