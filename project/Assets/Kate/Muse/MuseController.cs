using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseController : MonoBehaviour {

	public Transform idlePoint;
	public Transform spawnPoint;
	public Transform targetPoint;

	public float floatStrength = 1;
	
	private float originalY;
	private GuideToPoint guide;

	void Start () {
		guide = GetComponent<GuideToPoint>();
		originalY = transform.position.y;
	}
	
	void Update () {

		//check to see if the muse is moving to a point - only hover when not moving
		bool guiding = GetComponent<GuideToPoint>().guiding;
		if (!guiding){
			transform.position = new Vector3(transform.position.x, 
				originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
				transform.position.z);
		} else {
			originalY = transform.position.y;
		}

	}

	public void ActivateMuse() {
		guide.activation = true;
		guide.guiding = true;
		guide.target = idlePoint;
	}

	public void DeactivateMuse(){
		guide.activation = true;
		guide.guiding = true;
		guide.target = spawnPoint;
	}

	public void MuseGuide(){
		guide.guiding = true;
		guide.target = targetPoint;
	}
}

