using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuseNavigation : MonoBehaviour {

	public NavMeshAgent agent;

	Action storedCompletedEvent;
	NavMeshPathStatus lastStatus;
	public GameObject trail;
	public ParticleSystem particles;
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
		
		
		if(agent.gameObject.activeInHierarchy && agent.remainingDistance < 1f && agent.remainingDistance != 0) {
			
				agent.gameObject.SetActive(false);
				trail.SetActive(false);
				particles.gameObject.SetActive(false);
				if(storedCompletedEvent != null)
					storedCompletedEvent();
			
			
		}
		lastStatus = agent.pathStatus;
	}

	public void NavigateToPoint(Vector3 target, Action completedEvent = null) {
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

}
