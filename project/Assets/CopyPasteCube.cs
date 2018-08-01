using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CopyPasteCube : MonoBehaviour {

	public GameObject whiteBoardPrefab;

	public Leap.Unity.Interaction.Anchor anchor; //Anchor that the cube attaches to
	public WhiteboardContainer wc; //Data stored in the whiteboard
	Leap.Unity.Interaction.AnchorableBehaviour ab;

	void Start () {
		wc = GetComponent<WhiteboardContainer>();
		ab = GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>();
	}

	//When the copy paste cube is let go, it creates a whiteboard with the copy data and then returns to its anchor
	public void graspEnd() {
		//Creates whiteboard with copy data
		Whiteboard whiteboard = ((GameObject)Instantiate(whiteBoardPrefab, transform.position, transform.rotation)).GetComponentInChildren<Whiteboard>();
		wc.data.position = whiteboard.transform.root.position;
		wc.data.rotation = whiteboard.transform.root.rotation;
		whiteboard.loadData(wc.data);
		SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].addFeature(whiteboard.dataContainer.data);
		SaveSystem.instance.saveCurrentSave();
		
		whiteboard.orientRotation();

		//returns to anchor
		transform.position = anchor.transform.position;
		transform.rotation = anchor.transform.rotation;
		ab.anchor = anchor;
		ab.TryAttach();
		
	}
}
