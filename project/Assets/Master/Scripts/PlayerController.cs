using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;

	public bool debugMove = false;
	private float moveSpeed = 3f;

	public CapsuleCollider collisionCapsule;
	public Transform head;
	Rigidbody rig;

	bool isJitterWalking = false;
	float jitterWalkCooldown;

	void Awake() {
		instance = this;
	}

	void Start () {
        //DontDestroyOnLoad(gameObject);
		rig = GetComponent<Rigidbody>();
	}
	

	void Update () {
		if(debugMove == true) {
			Vector3 vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			moveInDirection(vel);
			
		}

		Vector3 localPos = head.localPosition;
		float height = localPos.y + (collisionCapsule.radius);
		height = Mathf.Clamp(height, 0f, 100f);
		collisionCapsule.height = height - 0.1f;
		collisionCapsule.center = new Vector3(0, height / -2f + collisionCapsule.radius, 0);
		collisionCapsule.transform.rotation = Quaternion.identity;


		//This works for some reason??
		
		Vector3 endPos = head.position;
		endPos.y = transform.position.y;
		endPos.y += 0.6f;
		if(Physics.CheckCapsule(head.transform.position, endPos, 0.35f, (1<<0))) {
			//print("HITTING");
			if(!isJitterWalking) {
				collisionCapsule.transform.parent.SetParent(transform);
				isJitterWalking = true;
			}
		}

		if(isJitterWalking) {
			Vector3 capPos = collisionCapsule.transform.parent.position;
			capPos.y = 0;
			Vector3 headPos = head.transform.position;
			headPos.y = 0;
			if(Vector3.Distance(capPos, headPos) > 0.15f) {
				head.transform.position = collisionCapsule.transform.parent.position;
				collisionCapsule.transform.parent.SetParent(head.transform);
				collisionCapsule.transform.parent.localPosition = Vector3.zero;
				isJitterWalking = false;
			}
		}
	}

	public void moveInDirection(Vector3 dir) {
		Vector3 vel = dir;
		vel.Normalize();
		vel *= moveSpeed;

		vel.y = rig.velocity.y < 0 ? rig.velocity.y * 2f : rig.velocity.y;

		RaycastHit hit;
		Vector3 headPositionFloor = head.position;
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
