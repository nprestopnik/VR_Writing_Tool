using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskManager : MonoBehaviour {

	public GameObject testNavPoint;

	public static DeskManager instance;

	public GameObject muse;
	public float pauseTime; //amount of time to pause to let user read initial text
	public GameObject deskTracker; //the tracked point on the desk
	public GameObject targetTracker; //the parent of the "target" for parking the desk
	public Transform parkMusePoint; //where the muse goes when the desk is parked
	public Transform moveMusePoint; //for the muse to stay with the desk while you are parking
	public GameObject lighthouse1; //the lighthouses - make visible to avoid collisions
	public GameObject lighthouse2;

	private SteamVR_TrackedObject deskTrackedObject;

	private Transform museCanvas; //the canvas attached to the muse
	private GuideToPoint museGuide; //the muse script responsible for moving the muse
	private MuseAppear museActivator; //the script responsible for making the muse appear/disappear
	private GameObject deskModel; //the visual desk object
	private GameObject deskTarget; //the "target" for desk placement
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
		
		deskTrackedObject = deskTracker.GetComponent<SteamVR_TrackedObject>();

		deskModel = deskTracker.transform.Find("Desk").gameObject;
		deskTarget = targetTracker.transform.Find("Desk").gameObject;
		
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

	public void StartDeskTask() {
		MuseManager.instance.museText.SetText("Follow me to your desk!", DeskStage10);
	}
	public void DeskStage10() {
		MuseManager.instance.museGuide.EnterMuse();
		MuseManager.instance.Pause(3f, DeskStage20);
	}
	public void DeskStage20() {
		MuseManager.instance.museGuide.GuideTo(moveMusePoint, DeskStage30);
	}
	public void DeskStage30() {
		deskModel.SetActive(true);
		deskParked.parked = false;
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		//movingDesk = true;
		MuseManager.instance.museText.SetText("Put your desk where you want it!");
	}


	public void StartNavgationTest() {
		MuseManager.instance.museText.SetText("Testing navigation!", NavStage10);
	}
	public void NavStage10() {
		MuseManager.instance.museGuide.EnterMuse();
		MuseManager.instance.Pause(3f, NavStage20);
	}
	public void NavStage20() {
		MuseManager.instance.museNavigator.NavigateToPoint(testNavPoint.transform.position, NavStage30);
	}
	public void NavStage30() {
		MuseManager.instance.museGuide.GuideTo(testNavPoint.transform);
	}


	public void ConfirmSet() {
		deskTrackedObject.enabled = false;

		movingDesk = false;
		//textMoveDesk.SetActive(false);
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

		deskTrackedObject.enabled = true;

		//show text saying to follow the muse and wait
		//textToParkLocation.SetActive(true);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		//pause so user can read text
		pausing = true;
		yield return new WaitForSeconds(pauseTime);
		pausing = false;

		//remove text and have muse guide to the target location
		//textToParkLocation.SetActive(false);
		//museGuide.GuideTo(parkMusePoint);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		//set the desk target active and show the text waiting for a confirmation of parking
		//textToFinish.SetActive(true);
		deskTarget.SetActive(true);
		lighthouse1.SetActive(true);
		lighthouse2.SetActive(true);
		
	}
	public void ConfirmPark() {
		deskParked.parked = true;
		//textToFinish.SetActive(false);
		deskModel.SetActive(false);
		deskTarget.SetActive(false);
		lighthouse1.SetActive(false);
		lighthouse2.SetActive(false);
		museActivator.ExitMuse();

		performing = false;
	}

}
