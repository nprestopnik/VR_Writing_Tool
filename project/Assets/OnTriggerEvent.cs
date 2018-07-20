using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour {

	public UnityEvent onTriggerEnter;

	void OnTriggerEnter(Collider other)
	{
		onTriggerEnter.Invoke();
	}
}
