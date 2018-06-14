using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParkButton : MonoBehaviour {

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

	public Transform deskParkMusePoint;
	public GameObject desk;
	public GameObject deskParkTarget;

	private Transform museCanvas;
	private GuideToPoint museGuide;

	private MuseAppear museActivator;

	private GameObject text1;
	private GameObject text2;

	//for testing?
	private bool performing = false;

	void Start() {
		museActivator = GetComponent<MuseAppear>();
		
		museCanvas = muse.transform.Find("Canvas");
		text1 = museCanvas.Find("Park1").gameObject;
		text2 = museCanvas.Find("Park2").gameObject;

		museGuide = muse.GetComponent<GuideToPoint>();
	}

	void Update () {
		
		if (Controller.GetHairTriggerDown() && !performing) {
			//this is the stuff that needs to happen when we get an actual desk button
			museActivator.EnterMuse();
			StartCoroutine(PerformParkTask());
		}

	}

	IEnumerator PerformParkTask() {
		
		//testing
		performing = true;

		text1.SetActive(true);
		yield return new WaitForSeconds(pauseTime);
		
		text1.SetActive(false);
		museGuide.GuideTo(deskParkMusePoint);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		
		text2.SetActive(true);
		deskParkTarget.SetActive(true);
		muse.SetActive(true);
		yield return new WaitUntil(()=> Controller.GetHairTriggerDown());
		
		text2.SetActive(false);
		desk.SetActive(false);
		deskParkTarget.SetActive(false);
		museActivator.ExitMuse();

		//testing
		performing = false;
	}
}