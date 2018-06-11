using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideToPoint : MonoBehaviour {

	public Transform target;
	public float guideSpeed = 5.0f;
	public float activationSpeed = 5.0f;
	public bool guiding = false;
	public bool activation = false;

	private Vector3 pos;
	
	void Start () {
		
	}
	
	void Update () {
		if (guiding) {
			if(transform.position != target.position) {
				if (activation) {
					pos = Vector3.MoveTowards(transform.position, target.position, activationSpeed * Time.deltaTime);
				}
				else {
					pos = Vector3.MoveTowards(transform.position, target.position, guideSpeed * Time.deltaTime);
				}
				GetComponent<Rigidbody>().MovePosition(pos);
			} else {
				guiding = false;
				activation = false;
			}
		}
	}
}
