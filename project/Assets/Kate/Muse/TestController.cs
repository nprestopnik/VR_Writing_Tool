using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

	public GameObject muse;
	public float pause;

	private MusePerformTask musePerformer;
	
	void Start () {
		
		musePerformer = muse.GetComponent<MusePerformTask>();

		StartCoroutine(MuseTest());

	}

	IEnumerator MuseTest() {

		yield return new WaitForSeconds(pause);
	
		musePerformer.PerformTask("Desk");

		yield return new WaitUntil(()=> !musePerformer.IsPerformingTask());
		yield return new WaitForSeconds(pause);

		musePerformer.PerformTask("Park");

	}
	
	
}
