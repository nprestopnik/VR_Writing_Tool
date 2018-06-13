using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusePerformTask : MonoBehaviour {

	public GameObject muse;
	public GameObject museCanvas;
	public float pauseTime;

	//VARIABLES NEEDED FOR TASKS

	[Header ("Task: Activate and Park Desk")]
	public Transform deskParkMusePoint;
	public GameObject desk;
	public GameObject deskParkTarget;

	//END VARIABLES NEEDED FOR TASKS

	private bool performingTask;

	private MuseAppear museActivation;
	private GuideToPoint museGuide;
	void Start() {
		museActivation = GetComponent<MuseAppear>();
		museGuide = GetComponent<GuideToPoint>();
	}

	//PUBLIC FUNCTION TO ACTIVATE NECESSARY MUSE TASK
	//FOR NEW TASK, MAKE CASE IN SWITCH AND CALL EXTERNAL FUNCTION
	public void PerformTask(string currentTask) {
		performingTask = true;
		switch(currentTask) {	
		case "Desk": 
			StartCoroutine(DeskTask());
			break;
		case "Park":
			StartCoroutine(ParkTask());
			break;
		}
	}

	public bool IsPerformingTask() {
		if (performingTask) return true;
		else return false;
	}

	//COROUTINES FOR MUSE TASKS

	//desk task: show user desk so they can position and use it
	IEnumerator DeskTask() {
		museActivation.EnterMuse();
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		var text1 = museCanvas.transform.Find("Desk1").gameObject;
		var	text2 = museCanvas.transform.Find("Desk2").gameObject;
		text1.SetActive(true);
		yield return new WaitForSeconds(pauseTime);
		
		text1.SetActive(false);
		museGuide.GuideTo(deskParkMusePoint);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		desk.SetActive(true);
		text2.SetActive(true);
		//will this be changed to a confirmation button instead of a set pause?
		yield return new WaitForSeconds(pauseTime);

		text2.SetActive(false);
		museActivation.ExitMuse();
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		performingTask = false;
	}

	IEnumerator ParkTask() {
		museActivation.EnterMuse();
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		var text1 = museCanvas.transform.Find("Park1").gameObject;
		var	text2 = museCanvas.transform.Find("Park2").gameObject;
		text1.SetActive(true);
		yield return new WaitForSeconds(pauseTime);
		
		text1.SetActive(false);
		museGuide.GuideTo(deskParkMusePoint);
		yield return new WaitUntil(()=> museGuide.IsAtTarget());
		
		deskParkTarget.SetActive(true);
		text2.SetActive(true);
		//will this be changed to a confirmation button instead of a set pause?
		yield return new WaitForSeconds(pauseTime);

		desk.SetActive(false);
		deskParkTarget.SetActive(false);
		text2.SetActive(false);
		museActivation.ExitMuse();
		yield return new WaitUntil(()=> museGuide.IsAtTarget());

		performingTask = false;
	}


	

	//END COROUTINES FOR MUSE TASKS
}
