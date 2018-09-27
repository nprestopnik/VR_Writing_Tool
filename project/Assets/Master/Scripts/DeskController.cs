using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

/*Purpose: Manages Desktop display and copy paste feature */
public class DeskController : MonoBehaviour {

	public static DeskController instance; //Singleton
	WhiteboardData lastData = null;

	VdmDesktop[] desktops;
	public Transform goalDesktopPosition;

	ClipboardListener cl;

	public GameObject copyPasteInteraction;
	public CopyPasteCube copyPasteCube;

	public GameObject whiteBoardPrefab;
	string lastCopy = "";

	public Text boardText;
	public RawImage boardImage;

    public SteamVR_TrackedController controller;

    public GameObject rightControllerModel;

    public float hideCopyPasteTimestamp;

	void Awake() {
		instance = this;
	}

	IEnumerator Start () {
		cl = GetComponent<ClipboardListener>();
		cl.onClipboardChange += onClipboardChange; //Adds clipboard event to the delegate

		desktops = new VdmDesktop[0];
		yield return new WaitForSeconds(0.1f); //Waits for desktops to be initialized (Bad way of doing this)
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
					desk.setDesktopTransform(goalDesktopPosition); //Sets desktop positions to the goal position. Should be adjusted for multiple monitors
				}
			}
		}

		foreach(Leap.Unity.RiggedHand hand in HandManager.instance.hands) {
			if(hand.isActiveAndEnabled) {
				//castFinger(hand); //Checks if the hand is pointing towards the monitor
			}
		}

        //castController(controller); //Check if the controller is pointing toward the monitor
        castController(rightControllerModel);

		//Manages the copyPasteCube being shown when a whiteboard is nearby
		if(hideCopyPasteTimestamp > Time.time) {
			copyPasteCube.gameObject.SetActive(false);
		} else {
			copyPasteCube.gameObject.SetActive(true);
		}

	}

	//Turns all desktops on or off
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


	//Checks if a finger is pointing towards a desktop
	public void castFinger(Leap.Unity.RiggedHand hand) {
		foreach(VdmDesktop desk in desktops) {
			Vector3 forwardVect = hand.Handedness == Leap.Unity.Chirality.Left ? hand.fingers[1].bones[3].right : hand.fingers[1].bones[3].right * -1f;
			Debug.Log(forwardVect);
			desk.CheckRaycast(hand.fingers[1].bones[3].position, forwardVect * -100f);
		}
	}

    //Check if a controller is pointing towards a desktop (using calibration controllers)
    public void castController(SteamVR_TrackedController controller){
        foreach (VdmDesktop desk in desktops){
            Vector3 forwardVect = new Vector3(controller.transform.rotation.x, controller.transform.rotation.y, controller.transform.rotation.z);
            Vector3 position = controller.transform.position;
            desk.CheckRaycast(position, forwardVect);
        }
    }

    //Check if a controller is pointing towards a desktop (using controller models);
    public void castController(GameObject obj){
        foreach (VdmDesktop desk in desktops){
			//Vector3 forwardVect = obj.transform.eulerAngles;
            Vector3 forwardVect = new Vector3(obj.transform.localRotation.x, obj.transform.localRotation.y, obj.transform.localRotation.z);
            Vector3 position = obj.transform.position;
            desk.CheckRaycast(position, forwardVect);
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

		if(lastData == null) {
			copyPasteInteraction.SetActive(true);
		}

		lastData = data;
		//Do stuff
		copyPasteCube.wc.data = lastData;
		GUIUtility.systemCopyBuffer = lastData.text;

		string text = data.text;
		string trimmedText = "file:///" + data.text.Trim(new Char[] {'"'}).Replace('\\', '/'); //Format text to be a path and add the file protocol

		//Check if the text is a link
		bool isUri = Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute); 
		bool isUriNoQuotes = Uri.IsWellFormedUriString(trimmedText, UriKind.RelativeOrAbsolute);

		if(!text.Equals("")) {
			if(isUri) { //If it is a link load it
				StartCoroutine(loadPicture(text));
				boardText.text = "";
				boardImage.gameObject.SetActive(true);
			} else if(isUriNoQuotes) { //If it is a link without quotes load it
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

	//Load a picture from a url
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
