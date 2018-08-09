/*
Hand Material Swap
Purpose: to swap the leap hand's between the opaque and transparent materials to facilitate proper fading out/in
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMaterialSwap : MonoBehaviour {

	public Material opaqueMat; //the material with rendering mode opaque
	public Material transparentMat; //the material with rendering mode transparent

	SkinnedMeshRenderer handRenderer; //the mesh renderer
	void Start() {
		handRenderer = GetComponent<SkinnedMeshRenderer>();
	}
	
	//set the hand's material to the one with the opaque rendering mode
	public void SetOpaque() {
		handRenderer.material = opaqueMat;
	}

	//set the hand's material to the one with the transparent rendering mode
	public void SetTransparent() {
		handRenderer.material = transparentMat;
	}
}
