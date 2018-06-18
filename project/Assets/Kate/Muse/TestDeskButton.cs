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
	public float pauseTime; //amount of time to pause to let user read initial text
	public Transform deskParkMusePoint; //where the muse goes when the desk is parked
	public GameObject deskTracker; //the tracked point on the desk
	public Transform deskMoveMusePoint; //for the muse to stay with the desk while you are parking
	public GameObject lighthouse1; //the lighthouses - make visible to avoid collisions
	public GameObject lighthouse2;


	private GameObject deskModel; //the visual desk object
	private Transform museCanvas; //the canvas attached to the muse
	private GuideToPoint museGuide; //the muse script responsible for moving the muse
	private MuseAppear museActivator; //the script responsible for making the muse appear/disappear
	private GameObject textToDesk; //the text (child of the muse canvas) that the muse will take you to the desk
	private GameObject textMoveDesk; //the text that says that you can put the desk where you want
	private DeskParked deskParked; //whether or not the desk is parked in its "inactive" location
	private bool movingDesk = false; //is the desk being moved
	private bool pausing = false; //is the muse paused in front of the user

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

		//keep the muse in place relative to moving objects - desk and head
		if(movingDesk) {
			muse.transform.position = deskMoveMusePoint.position;
		}
		if(pausing) {
			muse.transform.position = museGuide.target.position;
		}

	}

	IEnumerator PerformDeskTask() {
		
		//for testing only
		performing = true;

		//show the text to follow the muse and leave it there for the pause time
		textToDesk.SetActive(true);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		//pause to allow the user to read
		pausing = true;
		yield return new WaitForSeconds(pauseTime);
		pausing = false;
		
		//get rid of the first text and send muse to the desk
		textToDesk.SetActive(false);
		museGuide.GuideTo(deskParkMusePoint);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		//activate the desk and indicate that it is no longer parked
		//tell user to place desk and wait for confirmation that they're done
		deskModel.SetActive(true);
		deskParked.parked = false;
		textMoveDesk.SetActive(true);
		//make lighthouses visible, make muse follow desk until deactivated
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		movingDesk = true;
		yield return new WaitUntil(()=> Controller.GetHairTriggerDown());

		//desk is presumably parked where the user wants it
		//get rid of the text and lighthouses and get the muse to leave
		movingDesk = false;
		textMoveDesk.SetActive(false);
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		museActivator.ExitMuse();

		//for testing only
		performing = false;

	}
}
