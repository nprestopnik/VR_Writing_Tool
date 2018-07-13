using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class DeskController : MonoBehaviour {

	public static DeskController instance;
	WhiteboardData lastData;

	VdmDesktop[] desktops;
	public Transform goalDesktopPosition;

	ClipboardListener cl;

	public CopyPasteCube copyPasteCube;

	public GameObject whiteBoardPrefab;
	string lastCopy = "";

	public Text boardText;
	public RawImage boardImage;

	void Awake()
	{
		instance = this;
	}

	IEnumerator Start () {
		cl = GetComponent<ClipboardListener>();
		cl.onClipboardChange += onClipboardChange;

		desktops = new VdmDesktop[0];
		yield return new WaitForSeconds(0.1f);
		desktops = gameObject.GetComponentsInChildren<VdmDesktop>();


		

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
				castFinger(hand);
			}
		}

		// if(Input.GetKeyUp(KeyCode.O)) {
		// 	toggleDesktop();
		// }

		if(Input.GetKeyUp(KeyCode.P) && !lastCopy.Equals("")) {
			Whiteboard whiteboard = ((GameObject)Instantiate(whiteBoardPrefab, transform.position, transform.rotation)).GetComponentInChildren<Whiteboard>();
			WhiteboardData data = new WhiteboardData();
			data.position = whiteboard.transform.root.position;
			data.rotation = whiteboard.transform.root.rotation;
			data.text = lastCopy;
			whiteboard.loadData(data);
			SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].addFeature(whiteboard.dataContainer.data);
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

	public void castFinger(Leap.Unity.RiggedHand hand) {
		foreach(VdmDesktop desk in desktops) {
			Vector3 forwardVect = hand.Handedness == Leap.Unity.Chirality.Left ? hand.fingers[1].bones[3].right : hand.fingers[1].bones[3].right * -1f;
			desk.CheckRaycast(hand.fingers[1].bones[3].position, forwardVect * -100f);
		}
	}

	void onClipboardChange(string newCopy) {
		//print(newCopy);
		lastCopy = newCopy;
		updateClipboardData(new WhiteboardData(lastCopy));
	}

	public void updateClipboardData(WhiteboardData data) {
		lastData = data;
		//Do stuff
		copyPasteCube.wc.data = lastData;
		GUIUtility.systemCopyBuffer = lastData.text;

		string text = data.text;
		string trimmedText = "file:///" + data.text.Trim(new Char[] {'"'}).Replace('\\', '/');
		//print(trimmedText);

		bool isUri = Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute);
		bool isUriNoQuotes = Uri.IsWellFormedUriString(trimmedText, UriKind.RelativeOrAbsolute);

		if(!text.Equals("")) {
			if(isUri) {
				StartCoroutine(loadPicture(text));
				boardText.text = "";
				boardImage.gameObject.SetActive(true);
			} else if(isUriNoQuotes) {
				StartCoroutine(loadPicture(trimmedText));
				boardText.text = "";
				boardImage.gameObject.SetActive(true);
			} else {
				boardText.text = text;
				boardImage.texture = null;
				boardImage.gameObject.SetActive(false);
			}
		} else {
			boardText.text = "";
			boardImage.texture = null;
			boardImage.gameObject.SetActive(false);
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
	
}
