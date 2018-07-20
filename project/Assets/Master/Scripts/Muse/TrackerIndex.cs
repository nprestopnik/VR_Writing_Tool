/*
Tracker Index
Purpose: make sure the tracker is paired with the right index 
	so the desk actually shows up where the tracker is and not on a controller, for instance
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEditor;

public class TrackerIndex : MonoBehaviour {

	void Awake() {
		SetTrackerIndex();
	}

	//sets the tracked object script's index to the index corresponding to the actual index of the object named tracker
	public void SetTrackerIndex() {
		uint index = 0;
		var error = ETrackedPropertyError.TrackedProp_Success;
		for (uint i = 0; i < 16; i++) {
			var result = new System.Text.StringBuilder((int)64);
			OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
			if (result.ToString().Contains("tracker")) {
				index = i;
				break;
			}
		}
		GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index;

	}

}

/*
this editor makes an inspector button to reset the tracker's index to the actual tracker
this was used when trying different combinations of tracker/controller (see trackingCases.cs) and having to reset the index at runtimes 
 */
[CustomEditor(typeof(TrackerIndex))]
public class TrackerIndexEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        TrackerIndex script = (TrackerIndex)target;
        if(GUILayout.Button("Reset Index")) {
            script.SetTrackerIndex();
        }
    }
}
