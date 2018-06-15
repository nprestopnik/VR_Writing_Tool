using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public bool debugMove = false;
	private float moveSpeed = 5f;

	public CapsuleCollider collisionCapsule;
	public Transform head;
	Rigidbody rig;
	void Start () {
        //DontDestroyOnLoad(gameObject);
		rig = GetComponent<Rigidbody>();
	}
	

	void Update () {
		if(debugMove == true) {
			Vector3 vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			vel.Normalize();
			vel *= moveSpeed;
			vel = Quaternion.Euler(0, transform.eulerAngles.y, 0) * vel;
			vel.y = rig.velocity.y;
			rig.velocity = vel;
			
		}

		Vector3 localPos = head.localPosition;
		float height = localPos.y + (collisionCapsule.radius);
		height = Mathf.Clamp(height, 0f, 100f);
		collisionCapsule.height = height;
		collisionCapsule.center = new Vector3(0, height / -2f + collisionCapsule.radius, 0);
	}
}
