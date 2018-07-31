/*
Controller Model Activation
Purpose: for activating and deactivating the controller models so you can selectively see/not see them in the scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ControllerModelActivation : MonoBehaviour {

	public static ControllerModelActivation instance;

	public GameObject leftControllerModel; //the normal steamvr left controller model
	public GameObject rightControllerModel; //the normal steamvr right controller model

	public GameObject leftLeapController; //the leap vive-style controller, left
	public GameObject rightLeapController; //the leap vive-style controller, right

	public GameObject[] leapControllerColliders; //the colliders on the leap controllers
	//these are unparented from the leap controllers at runtime, and have to be set inactive separately
	//they can be found within the hierarchy of the leap controllers before entering play mode


	void Awake() {
		instance = this;
	}


	//set the controller models active
	public void ActivateControllers() {
		leftControllerModel.SetActive(true);
		rightControllerModel.SetActive(true);
		leftLeapController.SetActive(true);
		rightLeapController.SetActive(true);

		foreach(GameObject g in leapControllerColliders) {
			g.SetActive(true);
		}
	}

	//set the controller models inactive
	public void DeactivateControllers() {
		leftControllerModel.SetActive(false);
		rightControllerModel.SetActive(false);
		leftLeapController.SetActive(false);
		rightLeapController.SetActive(false);

		foreach(GameObject g in leapControllerColliders) {
			g.SetActive(false);
		}
	}
	
}

//editor for testing functions from inspector

[CustomEditor(typeof(ControllerModelActivation))]
public class ControllerModelEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        ControllerModelActivation script = (ControllerModelActivation)target;
        if(GUILayout.Button("Activate Controllers")) {
            script.ActivateControllers();
        }
		if(GUILayout.Button("Deactivate Controllers")) {
            script.DeactivateControllers();
        }
    }
}
