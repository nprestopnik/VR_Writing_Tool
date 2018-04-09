using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

	public SteamVR_TrackedObject otherControl;

	public bool grabbing;
	public bool otherGrab;

	private SteamVR_TrackedObject trackedObj;
	private LineRenderer lineRenderer;
	private Vector3 hitPoint;


	private Vector3[] reset = new Vector3[2];

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	private SteamVR_Controller.Device Controller2 {
		get { return SteamVR_Controller.Input((int)otherControl.index); }
	}
		

	private void scaleObject(RaycastHit hit, RaycastHit otherHit) {														

		GameObject toScale = hit.collider.gameObject.transform.parent.gameObject;

		RectTransform canvasTrans = toScale.GetComponent<RectTransform>();
//		Debug.Log (hit.point);
//		Debug.Log (otherHit.point);
//		Debug.Log (hit.point - otherHit.point);
		//Vector3 difference = hit.point - otherHit.point;

		canvasTrans.anchoredPosition3D = hit.point;


	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		lineRenderer = GetComponent<LineRenderer>();

		reset = new Vector3[2];
		reset[0] = new Vector3 (0, 0, 0);
		reset[1] = new Vector3 (0, 0, 0);

		grabbing = false;


	}

	void Start() {

	}

	void Update() {
		
		otherGrab = otherControl.GetComponent<LaserPointer>().grabbing;

		if (Controller.GetHairTrigger() && Controller2.GetHairTrigger()) {

			RaycastHit hit;
			RaycastHit otherHit;
			var points = new Vector3[2];

			if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100)) {
			//Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 10000);

				hitPoint = hit.point;
				points [0] = trackedObj.transform.position;
				points [1] = hitPoint;
				lineRenderer.SetPositions (points);

				if (hit.collider.gameObject.tag == "ScaleBlock") {
					grabbing = true;
				} else {
					grabbing = false;
				}
					

				if (grabbing == true && otherGrab == true) {
					Physics.Raycast (otherControl.transform.position, transform.forward, out otherHit, 100);
					scaleObject (hit, otherHit);
				}
					

			} else {
				lineRenderer.SetPositions(reset);
			}

		}




		if (Controller.GetHairTriggerUp() || Controller2.GetHairTriggerUp()) {
			
			lineRenderer.SetPositions(reset);
			grabbing = false;

		}

	}

}
