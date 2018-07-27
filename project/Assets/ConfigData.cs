using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigData {

	public List<Vector3> deskCalibrationPoints;
	public List<Vector3> chairCalibrationPoints;

	public ConfigData() {
		deskCalibrationPoints = new List<Vector3>(2);
		chairCalibrationPoints = new List<Vector3>(4);
	}
}
