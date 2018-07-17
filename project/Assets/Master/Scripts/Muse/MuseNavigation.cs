using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuseNavigation : MonoBehaviour {

	public NavMeshAgent agent;

	Action storedCompletedEvent;
	public GameObject trail;
	public ParticleSystem particles;

	
	public GameObject destinationCube;
	public Transform hallwayPoint;
	[HideInInspector]
	public bool arrivedAtHallway;
	[HideInInspector]
	public MeshRenderer destBlockMesh;
	[HideInInspector]
	public MeshRenderer destIconMesh;

	void Start() {
		destBlockMesh = destinationCube.transform.Find("block mesh").GetComponent<MeshRenderer>();
		destIconMesh = destinationCube.transform.Find("icon mesh").GetComponent<MeshRenderer>();
	}

	void Update() {
		// Debug.Log("Remaining distance: " + agent.remainingDistance);
		// Debug.Log("Path pending: " + agent.pathPending);
		// Debug.Log("Has path: " + agent.hasPath);
		// Debug.Log("Path status: " + agent.pathStatus);
		particles.transform.position = transform.position;

		if(agent.gameObject.activeInHierarchy) {
			
			if(Vector3.Distance(transform.position, SaveSystem.instance.transform.position) > 3f) {
				agent.isStopped = true;
			} else if(agent.isStopped) {
				agent.isStopped = false;
			}
		}
		
		if(MuseManager.instance.clearingMuse) {
			agent.gameObject.SetActive(false);
			trail.SetActive(false);
			particles.gameObject.SetActive(false);

			MuseManager.instance.clearingMuse = false;
		}
		
		if(agent.gameObject.activeInHierarchy && agent.remainingDistance < 1f && agent.remainingDistance != 0) {
			
				agent.gameObject.SetActive(false);
				trail.SetActive(false);
				particles.gameObject.SetActive(false);
				if(storedCompletedEvent != null)
					storedCompletedEvent();
			
		}

	}

	public void NavigateToPoint(Vector3 target, Action completedEvent = null) {
		if(MuseManager.instance.clearingMuse) {
			MuseManager.instance.clearingMuse = false;
			return;
		}	

		storedCompletedEvent = completedEvent;
		agent.gameObject.SetActive(true);
		trail.SetActive(true);
		particles.gameObject.SetActive(true);
		particles.Clear();

		NavMeshHit hit;
		if(NavMesh.SamplePosition(transform.position,out hit, 2f, NavMesh.AllAreas)) {
			agent.Warp(transform.position);
			agent.SetDestination(target);
			MuseManager.instance.museGuide.GuideTo(agent.transform);
		}

		
	}

	public void GetToHallway() {
		if(MuseManager.instance.clearingMuse) {
			MuseManager.instance.clearingMuse = false;
			return;
		}	

		StartCoroutine(PauseForExit());
	}
	IEnumerator PauseForExit() {
		yield return new WaitUntil(()=> arrivedAtHallway == true);
		MuseManager.instance.museGuide.ExitMuse();
	}

}
