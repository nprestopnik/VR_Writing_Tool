using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Activates target when hover has been triggered */
public class MenuProximityShow : MonoBehaviour {

	public GameObject target;

	private float showTimestamp;
	private float hoverTimestamp;

	//Time finger has to hover for target to display
	private float hoverShowDelay = 0.1f;
	//Time finger has to be gone for target to dissapear 
	private float showHideDelay = 0.5f;

	public bool alwaysOn = false;

	void Start () {
		target.SetActive(false);
	}

	void Update() {
		if(alwaysOn) { //If alwaysOn the target is always on
			target.SetActive(true);
		} else {
			if(showTimestamp < Time.time) {
				target.SetActive(false);
			} 
		}
		
	}

	//Start showing the target
	public void beginShow() {
		hoverTimestamp = Time.time + hoverShowDelay;
	}
	

	//Maintain showing the target
	public void stayShow() {
		if(hoverTimestamp < Time.time) { 
			target.SetActive(true);
			showTimestamp = Time.time + showHideDelay;
		}
	}

	public void endShow() {

	}
}
