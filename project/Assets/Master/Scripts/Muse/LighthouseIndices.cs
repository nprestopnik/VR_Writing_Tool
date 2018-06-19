using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LighthouseIndices : MonoBehaviour {

	void Awake() {
	
		SteamVR_TrackedObject lighthouse1 = transform.Find("Lighthouse 1").GetComponent<SteamVR_TrackedObject>();
		SteamVR_TrackedObject lighthouse2 = transform.Find("Lighthouse 2").GetComponent<SteamVR_TrackedObject>();
		bool set1 = false;
		bool set2 = false;

		uint index = 0;
		var error = ETrackedPropertyError.TrackedProp_Success;
		for (uint i = 0; i < 16; i++) {
			var result = new System.Text.StringBuilder((int)64);
			OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
			if (result.ToString().Contains("basestation")) {
				if (!set1) {
					index = i;
					lighthouse1.index = (SteamVR_TrackedObject.EIndex)index;
					set1 = true;
				} else if (!set2) {
					index = i;
					lighthouse2.index = (SteamVR_TrackedObject.EIndex)index;
					set2 = true;
					break;
				} 
			}
		}
		

	}

}
