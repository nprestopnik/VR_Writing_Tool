using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardMaterialSwapper : MonoBehaviour {

	public Whiteboard bb2;
	public int materialIndex;
	

	public void buttonPressed() {
		bb2.setMaterial(materialIndex);
	}
}
