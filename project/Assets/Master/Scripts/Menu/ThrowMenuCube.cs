/*
Throw Menu Cube
Purpose: handle the grabbing and throwing of menu/grabbable cubes
attach the appropriate functions to the leap interaction scripts
 */

using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Examples;
using Leap.Unity.Interaction;
using UnityEngine;

public class ThrowMenuCube : MonoBehaviour {

	public SimpleMatchAnchorScaleAndState anchorMatch;
	public bool movingMenu; //is this cube part of the hand menu (or some other hypothetical menu) that moves/tracks hands?

	private ActivateMenuCubeFunction activator;

	private Rigidbody rb;
	private AnchorableBehaviour ab;
	private InteractionBehaviour ib;

	[HideInInspector]
	public bool thisCubeGrasped = false; //is this particular cube being grasped, currently
	[HideInInspector]
	public float timeThrown = 0f; //the time at which this cube was thrown - used to determine when the cube will be activated
	[HideInInspector]
	public Transform cubeParent; //the cube's parent in the menu

	List<InteractionHand> graspingHands;

	void Start() {
		activator = GetComponent<ActivateMenuCubeFunction>();
		rb = GetComponent<Rigidbody>();
		ab = GetComponent<AnchorableBehaviour>();
		ib = GetComponent<InteractionBehaviour>();
		graspingHands = new List<InteractionHand>();
	}

	void Update() {
		if (ab.isAttached) {
			//make the cube move less when it is attached
			rb.drag = 5;
			rb.angularDrag = 5;
			//if this cube was just being used, set that it is no longer grasped, and no menu cube is in use
			if(MainMenu.cubeInUse && thisCubeGrasped) {
				thisCubeGrasped = false;
				MainMenu.cubeInUse = false;
			}
		} 

		//if some other menu cube is being grasped/thrown, don't let this cube be grabbed
		if (MainMenu.cubeInUse && !thisCubeGrasped) {
			ib.ignoreGrasping = true;
		} else {
			ib.ignoreGrasping = false;
		}
	}

	/*
	make sure that when this cube is grasped, it tells the menu that it is in use and makes sure this cube isn't flagged as having been thrown
	this deals with the case of letting go of a cube in the air but re-grabbing it so it doesn't activate
	leap interaction on grasp event
	*/
	public void Grasping() {
		MainMenu.cubeInUse = true;
		thisCubeGrasped = true;
		activator.thrown = false;

		//keep track of only the most recent hand grasping
		graspingHands.Clear();
		foreach(InteractionHand hand in ib.graspingHands) {
			graspingHands.Add(hand);
		}
	}


	/*
	throw the cube when it is no longer being grasped!
	on post try anchor on grasp end or whatever
	 */
	public void ThrowCube() {

		//if the hand that is grapsing the cube goes out of view, return the cube without activating it
		//to avoid accidental cube throwing when the hands disappear
		foreach(InteractionHand hand in graspingHands) {
			if(!hand.isActiveAndEnabled) {
				activator.ReturnCube();
				return;
			}
		}

		//make sure the cube isn't attached to the anchor before "throwing" it
		if (!ab.isAttached) {
			//update when the cube was thrown and let that sucker fly
			timeThrown = Time.time;
			rb.drag = 0;
			rb.angularDrag = 0;

			//unparent the cube from the menu if menu movement will make the cube freak out
			if(movingMenu) {
				cubeParent = transform.parent;
				transform.parent = null;
			}
			//if the cube is supposed to match an anchor state make sure it doesn't anymore so it doesn't disappear when it isn't supposed to
			if(anchorMatch) {
				anchorMatch.enabled = false;
			}
	
			activator.thrown = true;
		}
	}

}
