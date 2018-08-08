using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollider : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		print("collision with: " + collision.collider.name);
	}

}
