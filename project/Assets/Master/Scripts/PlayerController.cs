using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Handles player movement as well as other things(?) */
public class PlayerController : MonoBehaviour {

	public static PlayerController instance; //Singleton

	public bool debugMove = false;
	private float moveSpeed = 3f; //Player movespeed

	public CapsuleCollider collisionCapsule; //Capsule the player uses for collision
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
		if(debugMove == true) { //Don't think this works anymore
			Vector3 vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			moveInDirection(vel);
			
		}

		//Calculates capsule size based on HMD position
		Vector3 localPos = head.localPosition;
		float height = localPos.y + (collisionCapsule.radius);
		height = Mathf.Clamp(height, 0f, 100f);
		collisionCapsule.height = height - 0.23f;
		collisionCapsule.center = new Vector3(0, height / -2f + collisionCapsule.radius, 0);
		collisionCapsule.transform.rotation = Quaternion.identity;


		//This works for some reason??
		
		Vector3 endPos = head.position;
		endPos.y = transform.position.y;
		endPos.y += 0.6f;
		if(Physics.CheckCapsule(head.transform.position, endPos, 0.35f, (1<<0))) { //Checks overlap capsule for jitter walking
			//print("HITTING");
			if(!isJitterWalking) { //If overlaps with wall and not already jitter walking
				collisionCapsule.transform.parent.SetParent(transform); //Decouples the capsule from the player 
				isJitterWalking = true; 
			}
		}

		if(isJitterWalking) {
			Vector3 capPos = collisionCapsule.transform.parent.position;
			capPos.y = 0;
			Vector3 headPos = head.transform.position;
			headPos.y = 0;
			if(Vector3.Distance(capPos, headPos) > 0.15f) { //If the jitter walk is greater than 15cm
				head.transform.position = collisionCapsule.transform.parent.position; //Reposition player back to capsule
				collisionCapsule.transform.parent.SetParent(head.transform); //Recouple capsule to player
				collisionCapsule.transform.parent.localPosition = Vector3.zero; //Fix positioning
				isJitterWalking = false;
			}
		}
	}

	public void moveInDirection(Vector3 dir) {
		Vector3 vel = dir;
		vel.Normalize(); //Magnitude of 1
		vel *= moveSpeed; //Magnitude of moveSpeed

		vel.y = rig.velocity.y; //Y velocity is physics based

		RaycastHit hit;
		Vector3 headPositionFloor = head.position;
		if(Physics.SphereCast(headPositionFloor, collisionCapsule.radius, Vector3.down, out hit, collisionCapsule.height - collisionCapsule.radius + 0.3f, 1<<0)) {
			if(hit.normal.y < 0.98f && (dir.x + dir.y == 0)) { //Checking for stopping on a slanted surface
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
