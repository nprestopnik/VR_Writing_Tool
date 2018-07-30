using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Purpose: Keep track of all the hands */
public class HandManager : MonoBehaviour {

	public static HandManager instance;
	public Leap.Unity.RiggedHand[] hands;

	void Awake()
	{
		instance = this;
		hands = new Leap.Unity.RiggedHand[0];
		StartCoroutine(refreshHands()); 
	}
	
	IEnumerator refreshHands() {
		
		for(;;) { //Infinite loop refreshing the hands array every second
			hands = gameObject.GetComponentsInChildren<Leap.Unity.RiggedHand>();
			yield return new WaitForSeconds(1f);
		}
	}
}
