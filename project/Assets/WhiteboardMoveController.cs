using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardMoveController : MonoBehaviour {

	Leap.Unity.Interaction.AnchorableBehaviour ab;

	public Whiteboard wb;

	void Start () {
		ab = GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>();
		ab.anchorGroup = DeskManager.instance.anchorGroup;
	}
	
	void Update () {
		if(Vector3.SqrMagnitude(ab.GetNearestValidAnchor().transform.position - transform.position) < 0.2f) {
			DeskController.instance.hideCopyPasteTimestamp = Time.time + 0.1f;
		}
	}

	public void onAnchorLock() {
		DeskController.instance.updateClipboardData(wb.dataContainer.data);
		wb.removeWhiteboard();
	}
}
