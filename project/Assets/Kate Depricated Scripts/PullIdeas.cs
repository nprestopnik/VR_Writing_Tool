using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullIdeas : MonoBehaviour {

	public GameObject bubblePrefab;
	public Transform bubbleSpawn;

	private SteamVR_TrackedObject currentController;
	private Vector3 offset;

	private GameObject currentBubble;
	private GameObject collidingObject;

	private SteamVR_Controller.Device Controller {
		get { return SteamVR_Controller.Input((int)currentController.index); }
	}

	void Awake() {
		currentController = GetComponent<SteamVR_TrackedObject> ();
		offset = new Vector3 (0f, 0f, 0.3f);
	}

	private void SetCollidingObject(Collider col) {
		if (collidingObject || !col.GetComponent<Rigidbody>() || col.gameObject.name == "Capsule") {
			return;
		}
		collidingObject = col.gameObject;
	}

	public void OnTriggerEnter(Collider other) {
		SetCollidingObject(other);
	}

	public void OnTriggerStay(Collider other) {
		SetCollidingObject(other);
	}

	public void OnTriggerExit(Collider other) {
		if (!collidingObject) {
			return;
		}
		collidingObject = null;
	}

	private void CreateBubble(){
		currentBubble = (GameObject)Instantiate (bubblePrefab, bubbleSpawn.position, bubbleSpawn.rotation);
		currentBubble.transform.SetParent (currentController.transform);
		currentBubble.transform.localPosition += offset;

	}
		

	private void ReleaseBubble() {

		if(currentBubble){
			currentBubble.transform.parent = null;
			currentBubble.GetComponent<Rigidbody>().velocity = Controller.velocity;
			currentBubble.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
		}

	}
		
	void Update () {

		if (Controller.GetHairTriggerDown()) {
			if (collidingObject.name == "Head") {
				CreateBubble ();
			}
		}

		if (Controller.GetHairTriggerUp ()) {
			ReleaseBubble ();
		}
	}

}
