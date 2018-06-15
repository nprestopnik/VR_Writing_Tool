using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public bool debugMove = false;
	private float moveSpeed = 15f;

	public CapsuleCollider collisionCapsule;
	public Transform head;
	void Start () {
        //DontDestroyOnLoad(gameObject);
	}
	

	void Update () {
		if(debugMove == true) {
			transform.root.position += transform.root.right * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
			transform.root.position += transform.root.forward * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
			//transform.root.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0, Input.GetAxis("Vertical") * Time.deltaTime) * moveSpeed;
		}

		Vector3 localPos = head.localPosition;
		float height = localPos.y + (collisionCapsule.radius);
		height = Mathf.Clamp(height, 0f, 100f);
		collisionCapsule.height = height;
		collisionCapsule.center = new Vector3(0, height / -2f + collisionCapsule.radius, 0);
	}
}
