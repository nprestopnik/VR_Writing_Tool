/*
Activate Menu Cube Function
Purpose: exactly what it sounds like: make the cube do what it's supposed to at the right time, then return it to its place
This script should be attached to any throwable cubes so they are activated upon time/distance or collision
 */

using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;
using UnityEngine.Events;


public class ActivateMenuCubeFunction : MonoBehaviour {

	public bool thrown = false; //if this cube has been thrown for activation

	private Rigidbody rbCube; 
	private AnchorableBehaviour abCube;
	private ThrowMenuCube throwCube;
	public Transform anchor;

	//public float activeDistance; //the distance the cube should travel before activating
	public float activeSeconds;  //the amount of time the cube should fly before activating

	public UnityEvent cubeFunction; //add any methods that should be called when the cube is thrown


	void Start() {
		rbCube = GetComponent<Rigidbody>();
		abCube = GetComponent<AnchorableBehaviour>();
		throwCube = GetComponent<ThrowMenuCube>();
	}

	void Update() {
		//this first version activates the cube based on distance travelled. I'm leaving it here in case you ever want it
		// if (thrown && (Mathf.Abs(transform.position.x - anchor.position.x) > activeDistance
		// 	|| Mathf.Abs(transform.position.y - anchor.position.y) > activeDistance
		// 	|| Mathf.Abs(transform.position.z - anchor.position.z) > activeDistance)) {

		//make sure the cube has been thrown (not just grasped again) and that the alloted time has based before activating
		if(thrown && Time.time > throwCube.timeThrown + activeSeconds) {
			//these two lines are really only important for cubes that spawn objects
			//they pretty much tell the positioner script that they are the cube being thrown so it should use their position/velocity for positioning
			PositionThrownObject.instance.cubeInitPosition = transform.position;
			PositionThrownObject.instance.cubeInitVelocity = rbCube.velocity;
				
			cubeFunction.Invoke();
			thrown = false;
			ReturnCube();
		}
	}

	//If the cube has been thrown and collides with something, it will be activated
	void OnCollisionEnter() {
		if(thrown) {
			cubeFunction.Invoke();
			thrown = false;
			ReturnCube();
		}
	}

	//returns the cube to its original place in the menu lineup
	void ReturnCube() {
		//stop the cube's movement
		rbCube.velocity = new Vector3(0,0,0);
		rbCube.angularVelocity = new Vector3(0,0,0);
		rbCube.drag = 5;
		rbCube.angularDrag = 5;

		//if the cube was part of a menu that moves (namely, the hand menu), reparent it (it would have been unparented upon its throwing)
		if(throwCube.movingMenu) {
			transform.parent = throwCube.cubeParent;
		}
		//if the cube is set to match its anchor scale and state, reenable that script (it is disabled by throwing it)
		if(throwCube.anchorMatch) {
			throwCube.anchorMatch.enabled = true;
			transform.localScale = Vector3.zero;
		}

		//reattach the cube to its anchor and let everyone know it's back
		transform.position = anchor.position;
		abCube.TryAttach();
		MainMenu.cubeInUse = false;
		throwCube.thisCubeGrasped = false;
	}

}
