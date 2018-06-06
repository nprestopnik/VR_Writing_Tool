using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour {

	Leap.Unity.Interaction.InteractionButton button;
	public Leap.Unity.Interaction.InteractionSlider strokeSlider;
	public GameObject pointer;

	public GameObject boardScaler;
	public GameObject scaleHandle;

	public Material lMat;
	public float lineWidth = 0.01f;

	

	private LineRenderer currLineR;
	private LineData currData;
	private List<Vector3> points;
	private List<LineData> history;
	private List<LineData> redoHistory;

	private int numLines = 0;

	// Use this for initialization
	void Start () {
		button = GetComponent<Leap.Unity.Interaction.InteractionButton>();
		history = new List<LineData>();
		redoHistory = new List<LineData>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Update pointer position
		if (button.isHovered) {
			pointer.transform.position = button.primaryHoveringControllerPoint;
			pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, pointer.transform.localPosition.y, 0);
		
			pointer.transform.localPosition = new Vector3(Mathf.Clamp(pointer.transform.localPosition.x, -1, 0),
															Mathf.Clamp(pointer.transform.localPosition.y, 0, 1),
															0);
		}

		//Update pointer visuals
		pointer.GetComponent<MeshRenderer>().material = (lMat);
		SetGlobalScale(pointer.transform, Vector3.one * lineWidth);

		//Scale board scale to custom size
		boardScaler.transform.localScale = (scaleHandle.transform.localPosition);// * transform.root.localScale.x;
		boardScaler.transform.localScale = new Vector3(boardScaler.transform.localScale.x * -1 , boardScaler.transform.localScale.y, 1);

		//Lock scale handle in plane
		scaleHandle.transform.localPosition = new Vector3(scaleHandle.transform.localPosition.x, scaleHandle.transform.localPosition.y, 0);
		scaleHandle.transform.localRotation = Quaternion.identity;

		//Update line information if drawing
		if(currLineR != null) {
			Vector3 newPos = pointer.transform.localPosition;
			currLineR.SetPositions(points.ToArray());
			currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
			currLineR.transform.localScale = Vector3.one;
			currLineR.positionCount = points.Count;
			currLineR.SetPosition(currLineR.positionCount - 1, newPos);
			points.Add(newPos);
		}
	}

	//Called when finger makes contact with the board
	public void begin(){
		GameObject go = new GameObject (); 

		currData = go.AddComponent<LineData>();
		currLineR = go.AddComponent<LineRenderer>();
		currLineR.startWidth = lineWidth;
		currLineR.endWidth = lineWidth;
		currLineR.material = lMat;
		currLineR.useWorldSpace = false;
		currLineR.alignment = LineAlignment.Local;
		points = new List<Vector3>();
		points.Add(pointer.transform.localPosition);
		
		currLineR.sortingOrder = numLines;
		numLines++;

		redoHistory = new List<LineData>();

		go.transform.SetParent(transform.parent);
		go.transform.localPosition = Vector3.zero;
	}

	//Called when contact ends with the board
	public void end(){
		currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
		currData.lineWidth = lineWidth;
		currData.lMat = lMat;
		currData.points = points.ToArray();
		history.Add(currData);
		currLineR = null;
		currData = null;
	}

	//Used to set the line width
	public void setStroke(float value) {
		lineWidth = value;
	}

	public void strokeSliderUpdate() {
		lineWidth = strokeSlider.VerticalSliderValue;
	}

	//Used to set the line material
	public void setMaterial(Material m) {
		lMat = m;
	}

	public void SetGlobalScale (Transform transform, Vector3 globalScale)
	{
		transform.localScale = Vector3.one;
		transform.localScale = new Vector3 (globalScale.x/transform.lossyScale.x, globalScale.y/transform.lossyScale.y, globalScale.z/transform.lossyScale.z);
	}

	public void Undo() {
		LineData l = history[history.Count - 1];
		redoHistory.Add(l);
		Destroy(l.gameObject);
		history.RemoveAt(history.Count - 1);
	}

	public void Redo() {
		if(redoHistory.Count > 0) {
			LineData l = redoHistory[redoHistory.Count - 1];
			
			GameObject go = new GameObject (); 

			currData = go.AddComponent<LineData>();
			currData.lineWidth = l.lineWidth;
			currData.lMat = l.lMat;
			currData.points = l.points;

			currLineR = go.AddComponent<LineRenderer>();
			currLineR.startWidth = l.lineWidth;
			currLineR.endWidth = l.lineWidth;
			currLineR.material = l.lMat;
			currLineR.useWorldSpace = false;
			currLineR.alignment = LineAlignment.Local;
			//points = new List<Vector3>();
			//points.Add(pointer.transform.localPosition);
			
			currLineR.sortingOrder = numLines;
			numLines++;

			currLineR.positionCount = l.points.Length - 1;
			currLineR.SetPositions(l.points);
			

			go.transform.SetParent(transform.parent);
			go.transform.localPosition = Vector3.zero;
			currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
			currLineR.transform.localScale = Vector3.one;

			//end();
			redoHistory.RemoveAt(redoHistory.Count - 1);
			history.Add(currData);
			currLineR = null;
			currData = null;
		}	
		
	}
}
