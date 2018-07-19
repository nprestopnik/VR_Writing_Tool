using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCubesCallToPlace : MonoBehaviour {

	[Header("PUT ANCHORS AND CUBES IN MATCHING ORDER")]
	public GameObject[] anchoredCubes;
	public GameObject[] anchors;

	private MissingReferenceException missingAnchorMatch;

	void Start() {
		if (anchoredCubes.Length != anchors.Length) {
			throw missingAnchorMatch;
		}
	}

	void OnEnable() {
		
		for (int i = 0; i < anchors.Length; i++){
			anchoredCubes[i].transform.position = anchors[i].transform.position;
		}
		
	}


}
