using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard2 : MonoBehaviour {

	Leap.Unity.Interaction.InteractionButton button;

	public GameObject lineHolder;
	public Material lMat;
	public float lineWidth = 0.01f;

	public GameObject pointer;

	private LineRenderer currLineR;
	private List<Vector3> points;

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
		}


		if(currLineR != null) {
			Vector3 newPos = transform.root.rotation * Vector3.Scale(pointer.transform.localPosition, transform.root.localScale);
			currLineR.SetPositions(points.ToArray());
			currLineR.positionCount = points.Count;
			currLineR.SetPosition(currLineR.positionCount - 1, newPos);
			points.Add(newPos);
		}
	}

	public void begin(){
		print("begin");
		GameObject go = new GameObject (); 

		currLineR = go.AddComponent<LineRenderer>();
		currLineR.startWidth = lineWidth;
		currLineR.endWidth = lineWidth;
		currLineR.material = lMat;
		currLineR.useWorldSpace = false;
		points = new List<Vector3>();
		points.Add(transform.root.rotation * Vector3.Scale(pointer.transform.localPosition, transform.root.localScale));

		go.transform.SetParent(transform.root);
		go.transform.localPosition = Vector3.zero;
	}

	public void end(){
		print("end");
		currLineR = null;
	}
}
