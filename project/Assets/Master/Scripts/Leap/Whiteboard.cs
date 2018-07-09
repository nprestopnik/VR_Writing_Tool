using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Whiteboard : MonoBehaviour {

	Leap.Unity.Interaction.InteractionButton button;
	public Leap.Unity.Interaction.InteractionSlider strokeSlider;
	public GameObject pointer;

	public GameObject boardScaler;
	public GameObject scaleHandle;
	public Text boardText;
	public RawImage boardImage;

	//Original Material for the line
	public Material lMat;
	int lMatIndex;
	public Material[] lineMaterials;
	//Original line width
	public float lineWidth = 0.01f;


	//The line currently being drawn
	private LineRenderer currLineR;
	//Data for history and undo/redo
	private LineDataContainer currData;
	private List<Vector3> points;
	private List<LineDataContainer> history;
	private List<LineDataContainer> redoHistory;

	public List<LineData> lines;

	//Used for sorting order
	private int numLines = 0;

	// Use this for initialization
	void Start () {
		button = GetComponent<Leap.Unity.Interaction.InteractionButton>();
		history = new List<LineDataContainer>();
		redoHistory = new List<LineDataContainer>();
		//lines = new List<LineData>();
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
		go.tag = "BoardLine";

		currData = go.AddComponent<LineDataContainer>();//
		currData.data = new LineData();

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

		redoHistory = new List<LineDataContainer>();

		go.transform.SetParent(transform.parent);
		go.transform.localPosition = Vector3.zero;
	}

	//Called when contact ends with the board
	public void end(){
		currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
		currData.data.lineWidth = lineWidth;
		currData.data.lMatIndex = lMatIndex;
		currData.data.points = points.ToArray();
		currData.data.sortingOrder = numLines;
		history.Add(currData);
		lines.Add(currData.data);
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
	public void setMaterial(int index) {
		lMat = lineMaterials[index];
		lMatIndex = index;
	}

	public void SetGlobalScale (Transform transform, Vector3 globalScale)
	{
		transform.localScale = Vector3.one;
		transform.localScale = new Vector3 (globalScale.x/transform.lossyScale.x, globalScale.y/transform.lossyScale.y, globalScale.z/transform.lossyScale.z);
	}

	//Destroys the gameobject and stores its data in a list
	public void Undo() {
		LineData l = history[history.Count - 1].data;
		redoHistory.Add(history[history.Count - 1]);
		Destroy(history[history.Count - 1].gameObject);
		history.RemoveAt(history.Count - 1);
		lines.RemoveAt(lines.Count - 1);
	}


	//Takes the data from the history and creates a new gameobject line
	public void Redo() {
		if(redoHistory.Count > 0) {
			LineData l = redoHistory[redoHistory.Count - 1].data;
			
			GameObject go = new GameObject (); 
			go.tag = "BoardLine";

			currData = go.AddComponent<LineDataContainer>();//.data = new LineData();
			currData.data = new LineData();
			currData.data.lineWidth = l.lineWidth;
			currData.data.lMatIndex = l.lMatIndex;
			currData.data.points = l.points;
			currData.data.sortingOrder = l.sortingOrder;

			currLineR = go.AddComponent<LineRenderer>();
			currLineR.startWidth = l.lineWidth;
			currLineR.endWidth = l.lineWidth;
			currLineR.material = lineMaterials[l.lMatIndex];
			currLineR.useWorldSpace = false;
			currLineR.alignment = LineAlignment.Local;
			currLineR.sortingOrder = l.sortingOrder;
			
			//currLineR.sortingOrder = numLines;
			//numLines++;

			currLineR.positionCount = l.points.Length - 1;
			currLineR.SetPositions(l.points);
			

			go.transform.SetParent(transform.parent);
			go.transform.localPosition = Vector3.zero;
			currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
			currLineR.transform.localScale = Vector3.one;

			//end();
			redoHistory.RemoveAt(redoHistory.Count - 1);
			history.Add(currData);
			lines.Add(currData.data);
			currLineR = null;
			currData = null;
		}	
		
	}


	//Deletes all lines. Non-reversable
	public void clear() {
		foreach(Transform t in  transform.parent) {
			if(t.tag.Equals("BoardLine")) {
				// LineData l = t.gameObject.GetComponent<LineData>();
				// LineData lhistory = new LineData();
				// lhistory.points = l.points;
				// lhistory.lineWidth = l.lineWidth;
				// lhistory.lMat = l.lMat;
				// history.Add(lhistory);
				Destroy(t.gameObject);
			}
		}
	}

	IEnumerator loadPicture(string url)
    {
        // Start a download of the given URL
        using (WWW www = new WWW(url))
        {
            // Wait for download to complete
            yield return www;

            // assign texture
            boardImage.texture = www.texture;
        }
    }

	public void loadData(WhiteboardData data) {
		//load the lines and position

		lines = new List<LineData>(data.lines);

		transform.root.position = data.position;
		transform.root.rotation = data.rotation;

		//Scaling is probably broken right now
		boardScaler.transform.localScale = data.scale;
		scaleHandle.transform.localPosition = new Vector3(-1 * data.scale.x, data.scale.y, 0);
		
		string text = data.text;
		string trimmedText = "file:///" + data.text.Trim(new Char[] {'"'}).Replace('\\', '/');
		print(trimmedText);

		bool isUri = Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute);
		bool isUriNoQuotes = Uri.IsWellFormedUriString(trimmedText, UriKind.RelativeOrAbsolute);

		if(isUri) {
			StartCoroutine(loadPicture(text));
			boardText.text = "";
		} else if(isUriNoQuotes) {
			StartCoroutine(loadPicture(trimmedText));
			boardText.text = "";
		} else {
			boardText.text = text;
			boardImage.texture = null;
		}
		
		

		foreach(LineData l in data.lines) {
			
			GameObject go = new GameObject (); 
			go.tag = "BoardLine";

			currData = go.AddComponent<LineDataContainer>();//.data = new LineData();
			currData.data = new LineData();
			currData.data.lineWidth = l.lineWidth;
			currData.data.lMatIndex = l.lMatIndex;
			currData.data.points = l.points;
			currData.data.sortingOrder = l.sortingOrder;

			currLineR = go.AddComponent<LineRenderer>();
			currLineR.startWidth = l.lineWidth;
			currLineR.endWidth = l.lineWidth;
			currLineR.material = lineMaterials[l.lMatIndex];
			currLineR.useWorldSpace = false;
			currLineR.alignment = LineAlignment.Local;
			currLineR.sortingOrder = l.sortingOrder;
			
			//currLineR.sortingOrder = numLines;
			//numLines++;

			currLineR.positionCount = l.points.Length - 1;
			currLineR.SetPositions(l.points);
			

			go.transform.SetParent(transform.parent);
			go.transform.localPosition = Vector3.zero;
			currLineR.transform.localRotation = Quaternion.Euler(0,0,0);
			currLineR.transform.localScale = Vector3.one;

			//end();
			currLineR = null;
			currData = null;
		}
	}
}
