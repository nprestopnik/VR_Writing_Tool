using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeskController : MonoBehaviour {

	VdmDesktop[] desktops;
	public Transform goalDesktopPosition;

	ClipboardListener cl;

	public GameObject whiteBoardPrefab;
	string lastCopy = "";

	IEnumerator Start () {
		desktops = new VdmDesktop[0];
		yield return new WaitForSeconds(0.1f);
		desktops = gameObject.GetComponentsInChildren<VdmDesktop>();
		cl = GetComponent<ClipboardListener>();
		cl.onClipboardChange += onClipboardChange;

		//Turn on in the beginning
		toggleDesktop();
	}
	
	// Update is called once per frame
	void Update () {
		//GUIUtility.systemCopyBuffer;

		foreach(VdmDesktop desk in desktops) {
			if(desk != null) {
				if(desk.Visible()) {
					desk.setDesktopTransform(goalDesktopPosition);
				}
			}
		}

		foreach(Leap.Unity.RiggedHand hand in HandManager.instance.hands) {
			if(hand.isActiveAndEnabled) {
				castFinger(hand.fingers[1].bones[3]);
			}
		}

		if(Input.GetKeyUp(KeyCode.O)) {
			toggleDesktop();
		}

		if(Input.GetKeyUp(KeyCode.P) && !lastCopy.Equals("")) {
			WhiteboardContainer whiteboard = ((GameObject)Instantiate(whiteBoardPrefab, transform.position, transform.rotation)).GetComponentInChildren<WhiteboardContainer>();
			WhiteboardData data = new WhiteboardData();
			data.position = whiteboard.transform.root.position;
			data.rotation = whiteboard.transform.root.rotation;
			data.text = lastCopy;
			whiteboard.loadData(data);
			SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].addFeature(whiteboard.data);
			SaveSystem.instance.saveCurrentSave();
			SceneManager.MoveGameObjectToScene(whiteboard.gameObject, SceneManager.GetSceneByBuildIndex(SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].sceneID));
		}
	}

	public void toggleDesktop() {
		foreach(VdmDesktop desk in desktops) {
			if(desk != null) {
				if(desk.Visible()) {
					desk.Hide();
				} else {
					desk.Show();
				}
			}
		}
	}

	public void castFinger(Transform endBone) {
		foreach(VdmDesktop desk in desktops) {
			desk.CheckRaycast(endBone.position, endBone.right * -100f);
		}
	}

	void onClipboardChange(string newCopy) {
		print(newCopy);
		lastCopy = newCopy;
	}
	
}
