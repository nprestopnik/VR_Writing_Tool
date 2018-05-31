using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManagerEvents : MonoBehaviour {

	public Material lMat;

	//public Leap.Unity.RigidHand hand;

	//private LineRenderer currLine;
	private MeshLineRenderer currLine;

	private int numClicks = 0;
	public void beginDraw() {
		GameObject go = new GameObject (); 
		//currLine = go.AddComponent<LineRenderer> ();
		go.AddComponent<MeshFilter> ();
		go.AddComponent<MeshRenderer> ();
		currLine = go.AddComponent<MeshLineRenderer> ();

		currLine.lmat = lMat;

		//currLine.SetWidth (.1f, .1f);
		currLine.setWidth (.01f);
		numClicks = 0;
		go.transform.SetParent(transform.root);
		print("BEGIN DRAW");
	}

	public void LateUpdate() {
		if(currLine != null) {
			//currLine.AddPoint(hand.fingers[1].bones[3].transform.position);
			currLine.AddPoint(transform.position);
			numClicks++;
		}
		
	}
	
	public void endDraw() {
		currLine = null;
		print("END DRAW");
	}
}
