using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCreateCube : MonoBehaviour {

	public GameObject canvasPrefab;
	public Transform canvasSpawn;

	private SteamVR_TrackedObject trackedObj;
	private GameObject collidingObject;
	private GameObject objectInHand;

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	// Update is called once per frame
	void Update () {
		if (Controller.GetHairTriggerDown ()) {
			Instantiate (canvasPrefab, canvasSpawn.position, canvasSpawn.rotation);
		}

		if (Controller.GetHairTriggerUp ()) {
			
		}
	}
}
