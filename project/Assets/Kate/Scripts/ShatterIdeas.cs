using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterIdeas : MonoBehaviour {

	public GameObject fragments;

	void OnCollisionEnter(Collision col) {

		Transform location = gameObject.transform;

		Destroy (gameObject);

		Instantiate (fragments, location.position, location.rotation);


	}
}
