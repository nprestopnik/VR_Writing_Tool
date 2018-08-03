using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Used to force cross-scene interaction with leap motion interaction behaviors */
public class InteractionEnabler : MonoBehaviour {

	void Start () {
		Leap.Unity.Interaction.InteractionBehaviour[] list = GetComponents<Leap.Unity.Interaction.InteractionBehaviour>();
		foreach(Leap.Unity.Interaction.InteractionBehaviour item in list) {
			item.enabled = true;
		}		
	}
	
	void Update () {
		
	}
}
