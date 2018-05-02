using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDelete : MonoBehaviour {


	public void OnTriggerEnter(Collider col) {
		Destroy (col.gameObject);
	}


}
