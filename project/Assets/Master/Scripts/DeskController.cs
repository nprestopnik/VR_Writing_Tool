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

	public float hideCopyPasteTimestamp;

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


		boardText.text = "";
		boardImage.texture = null;
		boardImage.gameObject.SetActive(false);

		//Turn on in the beginning
		toggleDesktop();
	}
	
	// Update is called once per frame
	void Update () {

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

		if(hideCopyPasteTimestamp > Time.time) {
			copyPasteCube.gameObject.SetActive(false);
		} else {
			copyPasteCube.gameObject.SetActive(true);
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

	//Method for clipboard listener
	//Called every time the clipboard changes
	void onClipboardChange(string newCopy) {
		//print(newCopy);
		lastCopy = newCopy;
		updateClipboardData(new WhiteboardData(lastCopy));
	}


	//When clipboard data is changed, the clipboard image is updated
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
