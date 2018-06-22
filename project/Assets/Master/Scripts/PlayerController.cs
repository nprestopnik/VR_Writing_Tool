using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public bool debugMove = false;
	private float moveSpeed = 3f;

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

			RaycastHit hit;
			Vector3 headPositionFloor = head.position;
			//headPositionFloor.y = transform.position.y;
			if(Physics.SphereCast(headPositionFloor, collisionCapsule.radius, Vector3.down, out hit, collisionCapsule.height - collisionCapsule.radius + 0.3f, 1<<0)) {
				if(hit.normal.y < 0.98f && (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") == 0)) {
					vel.y = 0;
					rig.isKinematic = true;
				}
				else {
					rig.isKinematic = false;
				}
			} else {
				rig.isKinematic = false;
			}
			
			rig.velocity = vel;
			
		}

		Vector3 localPos = head.localPosition;
		float height = localPos.y + (collisionCapsule.radius);
		height = Mathf.Clamp(height, 0f, 100f);
		collisionCapsule.height = height;
		collisionCapsule.center = new Vector3(0, height / -2f + collisionCapsule.radius, 0);
		collisionCapsule.transform.rotation = Quaternion.identity;
	}

	public void moveInDirection(Vector3 dir) {
			Vector3 vel = dir;
			//print(dir);
			vel.Normalize();
			vel *= moveSpeed;
			//vel = Quaternion.Euler(0, transform.eulerAngles.y, 0) * vel;

			vel.y = rig.velocity.y;

			RaycastHit hit;
			Vector3 headPositionFloor = head.position;
			//headPositionFloor.y = transform.position.y;
			if(Physics.SphereCast(headPositionFloor, collisionCapsule.radius, Vector3.down, out hit, collisionCapsule.height - collisionCapsule.radius + 0.3f, 1<<0)) {
				if(hit.normal.y < 0.98f && (dir.x + dir.y == 0)) {
					vel.y = 0;
					rig.isKinematic = true;
				}
				else {
					rig.isKinematic = false;
				}
			} else {
				rig.isKinematic = false;
			}
			
			rig.velocity = vel;
	}
}
