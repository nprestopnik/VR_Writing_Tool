using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CalibrationStage {DesignateController = 0, CalibrateDesk = 1, CalibrateChair = 2}

public class Calibrator : MonoBehaviour {

	CalibrationStage currentStage = CalibrationStage.DesignateController;

	public desksize deskResizer;
	public ChairSizer chairResizer;
	public GameObject backPivotPrefab;
	public GameObject seatPivotPrefab;
	public GameObject[] stagePanels;

	public SteamVR_TrackedController rightController;
	public SteamVR_TrackedController leftController;
	SteamVR_TrackedController calibrationController;
	SteamVR_TrackedController chairController;
	public SteamVR_TrackedObject deskTracker;
	List<Vector3> deskCalibrationPoints;
	List<Vector3> chairCalibrationPoints;


	void OnEnable() {
		currentStage = CalibrationStage.DesignateController;
		deskCalibrationPoints = new List<Vector3>();
		chairCalibrationPoints = new List<Vector3>();
	}
	
	void Update () {
		foreach(GameObject g in stagePanels) {
			g.SetActive(false);
		}
		stagePanels[(int)currentStage].SetActive(true);

		if(currentStage == CalibrationStage.DesignateController) {
			if(rightController.triggerPressed) {
				calibrationController = rightController;
				chairController = leftController;
				calibrationController.TriggerClicked += triggerClicked;
				currentStage = CalibrationStage.CalibrateDesk;
			} else if(leftController.triggerPressed) {
				calibrationController = leftController;
				chairController = rightController;
				calibrationController.TriggerClicked += triggerClicked;
				currentStage = CalibrationStage.CalibrateDesk;
			}
		}
	}

	void triggerClicked(object sender, ClickedEventArgs e)
    {
        if(currentStage == CalibrationStage.CalibrateDesk) {
			if(deskCalibrationPoints.Count <= 2) {
				deskCalibrationPoints.Add(deskTracker.transform.InverseTransformPoint(calibrationController.transform.Find("Calibration Point").position));
				print(deskCalibrationPoints[deskCalibrationPoints.Count - 1]);
			}

			if(deskCalibrationPoints.Count == 2) {
				//math stuff
				float width, height, depth;
				height = deskTracker.transform.localPosition.y;
				width = deskCalibrationPoints[1].x - deskCalibrationPoints[0].x;
				depth = deskCalibrationPoints[0].y - deskCalibrationPoints[1].y;


				deskResizer.setScale(width, height, depth);

				currentStage = CalibrationStage.CalibrateChair;
			}
		} else if (currentStage == CalibrationStage.CalibrateChair) {
			if(chairCalibrationPoints.Count <= 4) {
				chairCalibrationPoints.Add(chairController.transform.InverseTransformPoint(calibrationController.transform.Find("Calibration Point").position));
			}

			if(chairCalibrationPoints.Count == 4) {
				//math stuff
				chairResizer = chairController.gameObject.AddComponent<ChairSizer>();
				chairResizer.backPivotPrefab = backPivotPrefab;
				chairResizer.seatPivotPrefab = seatPivotPrefab;
				chairResizer.setCalibrationPoints(chairCalibrationPoints);
				

				//CALIBRATION DONE
				// for(int i = 0; i < deskCalibrationPoints.Count; i++) {
				// 	PlayerPrefs.SetFloat("deskCalibrationPointX" + i, deskCalibrationPoints[i].x);
				// 	PlayerPrefs.SetFloat("deskCalibrationPointY" + i, deskCalibrationPoints[i].y);
				// 	PlayerPrefs.SetFloat("deskCalibrationPointZ" + i, deskCalibrationPoints[i].z);
				// }
				// for(int i = 0; i < chairCalibrationPoints.Count; i++) {
				// 	PlayerPrefs.SetFloat("chairCalibrationPointX" + i, chairCalibrationPoints[i].x);
				// 	PlayerPrefs.SetFloat("chairCalibrationPointY" + i, chairCalibrationPoints[i].y);
				// 	PlayerPrefs.SetFloat("chairCalibrationPointZ" + i, chairCalibrationPoints[i].z);
				// }
				// PlayerPrefs.Save();
				foreach(Transform t in chairController.transform) {
					t.gameObject.SetActive(false);
				}
				gameObject.SetActive(false);
			}
		}
    }

	public SteamVR_TrackedController getChairController() {
		return chairController;
	}
}
