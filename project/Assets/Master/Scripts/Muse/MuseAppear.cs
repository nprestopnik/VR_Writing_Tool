using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseAppear : MonoBehaviour {

	public GameObject muse;
	public float pause = 1.0f;

	private GuideToPoint guide;
	private MusePointManager points;
	private EnterFromDirection entryDirection;
	private Transform entryPoint;
	private GameObject exitText;

	void Awake() {
		guide = muse.GetComponent<GuideToPoint>();
		points = muse.GetComponent<MusePointManager>();
		entryDirection = points.entryDirection;

		switch((int)entryDirection) {
			//above
			case 0:
				entryPoint = points.abovePoint;
				break;
			//below
			case 1:
				entryPoint = points.belowPoint;
				break;
			//left
			case 2:
				entryPoint = points.leftPoint;
				break;
			//right
			case 3:
				entryPoint = points.rightPoint;
				break;
		}

		exitText = muse.transform.Find("Canvas").Find("Bye").gameObject;

	}

	void Update() {

	}
	
	public void EnterMuse() {
		muse.SetActive(true);
		muse.transform.position = entryPoint.position;
		guide.GuideTo(points.startPoint);
	}

	public void ExitMuse() {
		StartCoroutine(DeactivateMuse());
	}

	IEnumerator DeactivateMuse() {
		//exit message	
		exitText.SetActive(true);
		yield return new WaitForSeconds(pause);
		exitText.SetActive(false);

		guide.GuideTo(entryPoint);

		yield return new WaitUntil(()=> guide.IsAtTarget());
		//yield return new WaitForSeconds(pause);
		muse.SetActive(false);
	}
}
