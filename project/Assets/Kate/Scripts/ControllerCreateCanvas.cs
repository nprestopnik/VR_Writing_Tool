using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCreateCanvas : MonoBehaviour {

	public GameObject canvasPrefab;
	public Transform canvasSpawn;

	public SteamVR_TrackedObject otherControl;

	private SteamVR_TrackedObject trackedObj;
	private GameObject collidingObject;
	private GameObject objectInHand;

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	private SteamVR_Controller.Device Controller2 {
		get { return SteamVR_Controller.Input((int)otherControl.index); }
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	// Update is called once per frame
	void Update () {
		if (Controller.GetHairTriggerDown() && !Controller2.GetHairTrigger()) {
			GameObject g = (GameObject)Instantiate (canvasPrefab, canvasSpawn.position, canvasSpawn.rotation);
			g.transform.position = canvasSpawn.position;
		}
			
	}
}
