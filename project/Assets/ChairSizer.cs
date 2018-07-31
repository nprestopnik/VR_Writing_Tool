using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSizer : MonoBehaviour {


	public GameObject backPivotPrefab;
	public GameObject seatPivotPrefab;
	List<Vector3> calibrationPoints;


	public void setCalibrationPoints (List<Vector3> points) {
		calibrationPoints = points;
		calibrateChair();
	}

	
	void OnEnable()
	{
		//setCalibrationPoints(SaveSystem.instance.getConfigData().chairCalibrationPoints);
	}

	public void destroyChair() {
		foreach(Transform t in transform) {
			if(t.tag.Equals("SeatParts")) {
				Destroy(t.gameObject);
			} 
		}
	}

	void calibrateChair() {
		destroyChair();

		// foreach(Vector3 v in  calibrationPoints) {
		// 	GameObject g = ((GameObject)Instantiate(new GameObject(), v, Quaternion.identity));
		// 	g.transform.SetParent(transform);
		// 	g.transform.localPosition = v;
		// 	g.transform.localRotation = Quaternion.identity;
		// }
		

		GameObject chairBack = ((GameObject)Instantiate(backPivotPrefab, calibrationPoints[1], Quaternion.identity));
		chairBack.transform.SetParent(transform);
		chairBack.transform.localPosition = calibrationPoints[1];
		Vector3 backForward = calibrationPoints[2] - calibrationPoints[1];
		//backForward -= calibrationPoints[1];
		Vector3 backUp = calibrationPoints[0]  - calibrationPoints[1];
		//backUp -= calibrationPoints[1];
		chairBack.transform.localRotation = Quaternion.LookRotation(backForward, backUp);
		//scale z and x (z is the seam)
		chairBack.transform.localScale = new Vector3( 0.05f, backUp.magnitude, backForward.magnitude);


		GameObject chairSeat = ((GameObject)Instantiate(seatPivotPrefab, calibrationPoints[1], Quaternion.identity));
		chairSeat.transform.SetParent(transform);
		chairSeat.transform.localPosition = calibrationPoints[1];
		Vector3 seatForward = calibrationPoints[2] - calibrationPoints[1];
		//seatForward  -= calibrationPoints[1];
		Vector3 seatUp = calibrationPoints[3] - calibrationPoints[1];
		//seatUp  -= calibrationPoints[1];
		chairSeat.transform.localRotation = Quaternion.LookRotation(seatForward, seatUp);
		//scale x and z where z is the seam
		chairSeat.transform.localScale = new Vector3( 0.05f, seatUp.magnitude,seatForward.magnitude);
	}
	
	
}
