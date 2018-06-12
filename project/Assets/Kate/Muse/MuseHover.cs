using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseHover : MonoBehaviour {

	public float floatStrength = 0.1f;
	
	private float originalY;
	private GuideToPoint guide;

	void Start () {
		guide = GetComponent<GuideToPoint>();
		originalY = transform.position.y;
	}
	
	void Update () {

		//check to see if the muse is moving to a point - only hover when not moving
		bool guiding = GetComponent<GuideToPoint>().guiding;
		bool parked = GetComponent<GuideToPoint>().parked;
		if (!guiding && !parked){
			transform.position = new Vector3(transform.position.x, 
				originalY + ((float)Mathf.Sin(Time.time)/10 * floatStrength),
				transform.position.z);
		} else {
			originalY = transform.position.y;
		}

	}
}
