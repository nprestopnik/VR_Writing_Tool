using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFragments : MonoBehaviour {

	public float lifespan = 10f;

	void Start () {

		Destroy (gameObject, lifespan);

	}

	
}
