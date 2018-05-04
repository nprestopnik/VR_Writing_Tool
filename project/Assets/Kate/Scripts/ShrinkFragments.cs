using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkFragments : MonoBehaviour {

	public float targetScale = 0.0f;
	public float shrinkSpeed = 0.1f;

	void Start () {
		
	}

	void Update () {

		gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3 (targetScale, targetScale, targetScale), Time.deltaTime * shrinkSpeed);

	}
}
