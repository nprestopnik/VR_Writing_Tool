using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuProximityShow : MonoBehaviour {

	public GameObject target;

	private float showTimestamp;
	private float hoverTimestamp;

	private float hoverShowDelay = 0.1f;
	private float showHideDelay = 0.5f;

	void Start () {
		target.SetActive(false);
	}

	void Update() {
		if(showTimestamp < Time.time) {
			target.SetActive(false);
		}
	}

	public void beginShow() {
		//target.SetActive(true);
		//showCooldown = Time.time + 1f;
		hoverTimestamp = Time.time + hoverShowDelay;
	}

	

	public void stayShow() {
		if(hoverTimestamp < Time.time) { 
			target.SetActive(true);
			showTimestamp = Time.time + showHideDelay;
		}
	}

	public void endShow() {
		//target.SetActive(false);
		//showCooldown = Time.time + 1f;
	}
}
