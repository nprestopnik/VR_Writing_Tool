using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard2 : MonoBehaviour {

	Leap.Unity.Interaction.InteractionButton button;

	public GameObject lineHolder;
	public Material lMat;
	public float lineWidth = 0.01f;

	public GameObject pointer;


	//private LineRenderer currLine;
	private GraphicsLineRenderer currLine;
	private LineRenderer currLineR;

	private int numClicks = 0;
	//DrawLineManagerEvents dlme;

	// Use this for initialization
	void Start () {
		button = GetComponent<Leap.Unity.Interaction.InteractionButton>();
		//dlme = GetComponent<DrawLineManagerEvents>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (button.isHovered) {
			pointer.transform.position = button.primaryHoveringControllerPoint;
			pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, pointer.transform.localPosition.y, 0);

			//currLine.SetVertexCount (numClicks + 1);
			//currLine.SetPosition (numClicks, trackedObj.transform.position);

			// if(button.primaryHoverDistance <= 0.1f) {
			// 	currLine.AddPoint(button.primaryHoveringControllerPoint);
			// 	numClicks++;
			// } else {
			// 	currLine = null;
			// 	numClicks = 0;
			// }
			
			
		}

		if(currLine != null) {
			


			//currLine.AddPoint(hand.fingers[1].bones[3].transform.position);
			currLine.AddPoint(pointer.transform.position);
			numClicks++;

			if( (transform.worldToLocalMatrix * button.primaryHoveringController.velocity).z < -15f) {
				//end();
			}
		}

		if(currLineR != null) {

		}
	}

	public void begin(){
		print("begin");
		GameObject go = new GameObject (); 
		go.AddComponent<MeshFilter> ();
		go.AddComponent<MeshRenderer> ();
		currLine = go.AddComponent<GraphicsLineRenderer> ();

		currLine.lmat = lMat;

		//currLine.SetWidth (.1f, .1f);
		currLine.SetWidth (lineWidth);
		numClicks = 0;

		// currLineR = go.AddComponent<LineRenderer>();
		// currLineR.startWidth = lineWidth;
		// currLineR.endWidth = lineWidth;


		go.transform.SetParent(transform.root);
	}

	public void end(){
		print("end");
		currLine = null;
		currLineR = null;
	}
}
