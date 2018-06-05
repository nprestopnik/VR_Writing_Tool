using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard2 : MonoBehaviour {

	Leap.Unity.Interaction.InteractionButton button;
	public Leap.Unity.Interaction.InteractionSlider strokeSlider;

	public GameObject boardScaler;
	public GameObject scaleHandle;

	public GameObject lineHolder;
	public Material lMat;
	public float lineWidth = 0.01f;

	public GameObject pointer;

	private LineRenderer currLineR;
	private List<Vector3> points;

	private int numClicks = 0;

	// Use this for initialization
	void Start () {
		button = GetComponent<Leap.Unity.Interaction.InteractionButton>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (button.isHovered) {
			pointer.transform.position = button.primaryHoveringControllerPoint;
			pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, pointer.transform.localPosition.y, 0);
		}

		pointer.GetComponent<MeshRenderer>().material = (lMat);
		SetGlobalScale(pointer.transform, Vector3.one * lineWidth);

		boardScaler.transform.localScale = (scaleHandle.transform.localPosition);// * transform.root.localScale.x;
		boardScaler.transform.localScale = new Vector3(boardScaler.transform.localScale.x * -1 , boardScaler.transform.localScale.y, 1);
		//SetGlobalScale(boardScaler.transform, scaleHandle.transform.localPosition);
		scaleHandle.transform.localPosition = new Vector3(scaleHandle.transform.localPosition.x, scaleHandle.transform.localPosition.y, 0);
		scaleHandle.transform.localRotation = Quaternion.identity;

		if(currLineR != null) {
			Vector3 newPos = pointer.transform.localPosition;//transform.root.rotation * Vector3.Scale(pointer.transform.localPosition, transform.root.localScale) - (transform.forward * lineWidth / 2);
			currLineR.SetPositions(points.ToArray());
			currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
			currLineR.transform.localScale = Vector3.one;
			currLineR.positionCount = points.Count;
			currLineR.SetPosition(currLineR.positionCount - 1, newPos);
			points.Add(newPos);
		}
	}

	public void begin(){
		//print("begin");
		GameObject go = new GameObject (); 

		currLineR = go.AddComponent<LineRenderer>();
		currLineR.startWidth = lineWidth;
		currLineR.endWidth = lineWidth;
		currLineR.material = lMat;
		currLineR.useWorldSpace = false;
		currLineR.alignment = LineAlignment.Local;
		points = new List<Vector3>();
		points.Add(pointer.transform.localPosition);
		

		go.transform.SetParent(transform.parent);
		go.transform.localPosition = Vector3.zero;
	}

	public void end(){
		//print("end");
		//currLineR.transform.SetParent(transform.parent);
		//currLineR.transform.localScale = Vector3.zero;
		currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
		//currLineR.transform.localPosition = transform.root.position - transform.position;
		currLineR = null;
	}

	public void setStroke(float value) {
		lineWidth = value;
	}

	public void strokeSliderUpdate() {
		lineWidth = strokeSlider.VerticalSliderValue;
	}

	public void setMaterial(Material m) {
		lMat = m;
	}

	public void SetGlobalScale (Transform transform, Vector3 globalScale)
	{
		transform.localScale = Vector3.one;
		transform.localScale = new Vector3 (globalScale.x/transform.lossyScale.x, globalScale.y/transform.lossyScale.y, globalScale.z/transform.lossyScale.z);
	}
}
