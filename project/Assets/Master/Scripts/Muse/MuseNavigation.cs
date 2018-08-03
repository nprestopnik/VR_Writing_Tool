﻿/*
Muse Navigation
Purpose: a fancier guide system than guide to point, for taking you somewhere that requires more moving
as opposed to guide to point, which will take the muse directly to a given target,
	the navigator will have the muse follow a path that the user can follow through the world
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuseNavigation : MonoBehaviour {

	public NavMeshAgent agent;

	Action storedCompletedEvent; //the completed event sent with navigation call

	
	public GameObject destinationCube; //the cube above the hallway that shows your next destination
	public Transform hallwayPoint; //the point in front of the hallway where the muse will stop so you can go through
	[HideInInspector]
	public bool arrivedAtHallway; //whether or not the use has reached the hallway to read the muse's message and go through
	[HideInInspector]
	public MeshRenderer destBlockMesh; //the meshes for the destination cube
	[HideInInspector]
	public MeshRenderer destIconMesh;

	void Start() {
		destBlockMesh = destinationCube.transform.Find("block mesh").GetComponent<MeshRenderer>();
		destIconMesh = destinationCube.transform.Find("icon mesh").GetComponent<MeshRenderer>();
	}

	void Update() {
		Debug.DrawLine(agent.transform.position, agent.destination, Color.red);
		//keep the particles with the muse
		
		//agent.transform.rotation = Quaternion.identity;

		if(agent.gameObject.activeInHierarchy) {
			//if the muse has gotten too far from the user, stop and wait for them
			if(Vector3.Distance(transform.position, SaveSystem.instance.transform.position) > 3f) {
				agent.isStopped = true;
			} else if(agent.isStopped) {
				agent.isStopped = false;
			}
		}
		
		
		//if the muse gets close to its target, turn off all its navigation things and start the completed event
		if(agent.gameObject.activeInHierarchy && agent.remainingDistance < 1f && agent.remainingDistance != 0) {
			
				agent.gameObject.SetActive(false);
				//MuseManager.instance.SetEffectsActive(false);
				if(storedCompletedEvent != null)
					storedCompletedEvent();
			
		}

	}

	//make the muse take you somewhere along a path that you could actually walk
	//give her a target and something to do when she's done (if you want) and she will take you where you need to go
	//there she go
	public void NavigateToPoint(Vector3 target, Action completedEvent = null) {
	
		//activate all those funky navigation effects (and the actual nav agent of course)
		storedCompletedEvent = completedEvent;
		
		//set up the nav mesh agent so the muse gets onto the mesh and the agent can work 
		NavMeshHit hit;
		if(NavMesh.SamplePosition(transform.position, out hit, 30f, NavMesh.AllAreas)) {
			
			Vector3 newPos = transform.position;
			newPos.y = SaveSystem.instance.transform.position.y + 0.5f;
			agent.Warp(hit.position);
			agent.gameObject.SetActive(true);
			//agent.ResetPath();
			agent.destination = (target);
			
			
			
			//have the muse follow the agent as it goes along the mesh to its destination
			MuseManager.instance.museGuide.GuideTo(agent.transform.GetChild(0));
		}

		
	}


	public void NavigateToHallway() {
		MuseManager.instance.museText.SetText("Your destination has been loaded!\nFollow me to the hallway!", startRoomGuide);
	}
	
	void startRoomGuide() {
		//use the muse callback system to bring the muse in, have it navigate to a point, and wait at the hallway with the right text
		MuseManager.instance.museGuide.EnterMuse(); 
		MuseManager.instance.Pause(2f, ()=> NavigateToPoint(hallwayPoint.position, 
			()=> MuseManager.instance.museText.SetText("Go through the hallway\nto the selected room!", 
			()=> GetToHallway())));
	}

	//make the muse wait for the user to get to the hallway before exiting		
	public void GetToHallway() {	

		StartCoroutine(PauseForExit());
	}
	IEnumerator PauseForExit() {
		//wait until the person gets to the hallway and hits the trigger
		yield return new WaitUntil(()=> arrivedAtHallway == true);
		MuseManager.instance.museGuide.ExitMuse();
	}

}
