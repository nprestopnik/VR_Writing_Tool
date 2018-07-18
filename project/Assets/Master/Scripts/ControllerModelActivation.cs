using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ControllerModelActivation : MonoBehaviour {

	public GameObject leftControllerModel;
	public GameObject rightControllerModel;

	public GameObject leftLeapController;
	public GameObject rightLeapController;

	public GameObject[] leapControllerColliders;


	public void ActivateControllers() {
		leftControllerModel.SetActive(true);
		rightControllerModel.SetActive(true);
		leftLeapController.SetActive(true);
		rightLeapController.SetActive(true);

		foreach(GameObject g in leapControllerColliders) {
			g.SetActive(true);
		}
	}

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

//for testing functions from inspector

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
