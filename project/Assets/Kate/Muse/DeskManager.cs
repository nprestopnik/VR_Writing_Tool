using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskManager : MonoBehaviour {

	public static DeskManager instance;

	public GameObject muse;
	public float pauseTime; //amount of time to pause to let user read initial text
	public GameObject deskTracker; //the tracked point on the desk
	public GameObject targetTracker; //the parent of the "target" for parking the desk
	public Transform parkMusePoint; //where the muse goes when the desk is parked
	public Transform moveMusePoint; //for the muse to stay with the desk while you are parking
	public GameObject lighthouse1; //the lighthouses - make visible to avoid collisions
	public GameObject lighthouse2;

	private Transform museCanvas; //the canvas attached to the muse
	private GuideToPoint museGuide; //the muse script responsible for moving the muse
	private MuseAppear museActivator; //the script responsible for making the muse appear/disappear
	private GameObject deskModel; //the visual desk object
	private GameObject deskTarget; //the "target" for desk placement
	private GameObject textToDesk; //the text (child of the muse canvas) that the muse will take you to the desk
	private GameObject textMoveDesk; //the text that says that you can put the desk where you want
	private GameObject textToParkLocation; //text to show the user where to put the desk when done
	private GameObject textToFinish; //confirmation that they're done with the desk
	private DeskParked deskParked; //whether or not the desk is parked in its "inactive" location
	private bool movingDesk = false; //is the desk being moved
	private bool pausing = false; //is the muse paused in front of the user
	private bool performing = false;

	void Awake () {
		instance = this;
	}

	void Start() {
		museActivator = GetComponent<MuseAppear>();
		museGuide = muse.GetComponent<GuideToPoint>();
		deskParked = deskTracker.GetComponent<DeskParked>();

		deskModel = deskTracker.transform.Find("Desk").gameObject;
		deskTarget = targetTracker.transform.Find("Desk").gameObject;
		
		museCanvas = muse.transform.Find("Canvas");
		textToDesk = museCanvas.Find("Desk1").gameObject;
		textMoveDesk = museCanvas.Find("Desk2").gameObject;	
		textToParkLocation = museCanvas.Find("Park1").gameObject;
		textToFinish = museCanvas.Find("Park2").gameObject;
	}

	void Update () {
		
		//keep the muse in place relative to moving objects - desk and head
		if(movingDesk) {
			muse.transform.position = moveMusePoint.position;
		}
		if(pausing) {
			muse.transform.position = museGuide.target.position;
		}

	}

	public void DeskTask() {
		if (!performing) {
			museActivator.EnterMuse();
			StartCoroutine(PerformDeskTask());
		}
	}
	IEnumerator PerformDeskTask() {

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
		museGuide.GuideTo(parkMusePoint);
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

	}
	public void ConfirmSet() {
		movingDesk = false;
		textMoveDesk.SetActive(false);
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		museActivator.ExitMuse();

		performing = false;
	}

	public void ParkTask() {
		if (!performing) {
			museActivator.EnterMuse();
			StartCoroutine(PerformParkTask());
		}
	}
	IEnumerator PerformParkTask() {

		performing = true;

		//show text saying to follow the muse and wait
		textToParkLocation.SetActive(true);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		//pause so user can read text
		pausing = true;
		yield return new WaitForSeconds(pauseTime);
		pausing = false;

		//remove text and have muse guide to the target location
		textToParkLocation.SetActive(false);
		museGuide.GuideTo(parkMusePoint);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		//set the desk target active and show the text waiting for a confirmation of parking
		textToFinish.SetActive(true);
		deskTarget.SetActive(true);
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		
	}
	public void ConfirmPark() {
		deskParked.parked = true;
		textToFinish.SetActive(false);
		deskModel.SetActive(false);
		deskTarget.SetActive(false);
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		museActivator.ExitMuse();

		performing = false;
	}

}
