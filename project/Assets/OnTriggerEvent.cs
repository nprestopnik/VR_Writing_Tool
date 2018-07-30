using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*Purpose: A simple event caller that calls the unity event OnTriggerEnter
 (Could add more events for other ontrigger events) */
public class OnTriggerEvent : MonoBehaviour {

	public UnityEvent onTriggerEnter; //Creates Unity inspector event

	void OnTriggerEnter(Collider other)
	{
		onTriggerEnter.Invoke(); //Invokes event when trigger happens
	}
}
