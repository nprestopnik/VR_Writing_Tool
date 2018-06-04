using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardMaterialSwapper : MonoBehaviour {

	public Blackboard2 bb2;
	public Material m;
	

	public void buttonPressed() {
		bb2.setMaterial(m);
	}
}
