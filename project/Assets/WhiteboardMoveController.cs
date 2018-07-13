using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardMoveController : MonoBehaviour {

	Leap.Unity.Interaction.AnchorableBehaviour ab;

	public Whiteboard wb;

	void Start () {
		ab = GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>();
		//ab.anchor = DeskManager.instance.anchor;
		ab.anchorGroup = DeskManager.instance.anchorGroup;
	}
	
	void Update () {
		
	}

	public void onAnchorLock() {
		DeskController.instance.updateClipboardData(wb.dataContainer.data);
		wb.removeWhiteboard();
	}
}
