using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

	public SteamVR_TrackedObject otherControl;
	public GameObject otherControlObject;

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
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void SetCollidingObject(Collider col) {
		if (collidingObject || !col.GetComponent<Rigidbody>()) {
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

	private void GrabObject(){
		objectInHand = collidingObject;
		collidingObject = null;

		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	private FixedJoint AddFixedJoint(){
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject() {
		if (GetComponent<FixedJoint>()) {
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());

			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
		}
		objectInHand = null;
			
	}
		
	
	// Update is called once per frame
	void Update () {
		if (Controller.GetHairTriggerDown ()) {
			if (collidingObject) {
				GrabObject ();
			} 
		}
			

		if (Controller.GetHairTriggerUp ()) {
			if (objectInHand) {
				ReleaseObject ();
			}
		}

//		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
//		{
//			Vector2 touchpad = (Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0));
//
//			if (objectInHand) {
//				if (touchpad.y > 0.7f) {
//					objectInHand.transform.localScale += new Vector3 (0.1F, 0.1f, 0.1f);
//				} else if (touchpad.y < 0.7f) {
//					objectInHand.transform.localScale -= new Vector3 (0.1F, 0.1f, 0.1f);
//				}
//			}
//		}
	}
}
