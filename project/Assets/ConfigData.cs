using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Data structure for the data that is saved in the config file
public class ConfigData {

	public List<Vector3> deskCalibrationPoints; //Points saved from desk calibration
	public List<Vector3> chairCalibrationPoints; //Points saved from chair calibrations

	public ConfigData() {
		deskCalibrationPoints = new List<Vector3>(2);
		chairCalibrationPoints = new List<Vector3>(4);
	}
}
