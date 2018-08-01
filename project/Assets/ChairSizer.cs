using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Purpose: Creates and sizes chair based on calibration data */
public class ChairSizer : MonoBehaviour {

	//Prefabs used to create the back and seat of the chair
	public GameObject backPivotPrefab;
	public GameObject seatPivotPrefab;
	List<Vector3> calibrationPoints; //Calibration points used to size chair


	//Sets points and calibrates chair
	public void setCalibrationPoints (List<Vector3> points) {
		calibrationPoints = points;
		calibrateChair();
	}

	//Destroys all chair parts on the controller
	public void destroyChair() {
		foreach(Transform t in transform) {
			if(t.tag.Equals("SeatParts")) {
				Destroy(t.gameObject);
			} 
		}
	}

	//Calibrates chair based on the assumption that there are 4 calibration points
	//Calibration is done: Top Left, left seam, right seam, bottom left
	void calibrateChair() {
		destroyChair(); //Destroys chair in case there was already one there
		
		GameObject chairBack = ((GameObject)Instantiate(backPivotPrefab, calibrationPoints[1], Quaternion.identity)); //Creates a chair back objer
		chairBack.transform.SetParent(transform); //Sets the chair back to be a child of the controller
		chairBack.transform.localPosition = calibrationPoints[1]; //Positions the back of the chair at the inner left seam of the chair
		Vector3 backForward = calibrationPoints[2] - calibrationPoints[1]; //Calculates forward vector
		Vector3 backUp = calibrationPoints[0]  - calibrationPoints[1]; //Calculates the up vector
		chairBack.transform.localRotation = Quaternion.LookRotation(backForward, backUp); //Rotates the back of the chair to face the forward vector with up facing the up vector
		//scale z and x (z is the seam)
		chairBack.transform.localScale = new Vector3( 0.05f, backUp.magnitude, backForward.magnitude);


		GameObject chairSeat = ((GameObject)Instantiate(seatPivotPrefab, calibrationPoints[1], Quaternion.identity)); //Creates a chair seat prefab
		chairSeat.transform.SetParent(transform); //Sets the chair seat to be a child of the controller
		chairSeat.transform.localPosition = calibrationPoints[1]; //Positions the seat at the inner left seam of the chair
		Vector3 seatForward = calibrationPoints[2] - calibrationPoints[1]; //Calculates the forward vector
		Vector3 seatUp = calibrationPoints[3] - calibrationPoints[1]; //Calculates the up vector
		chairSeat.transform.localRotation = Quaternion.LookRotation(seatForward, seatUp); //Rotates the seat of the chair to face the forward vector with up facing the up vector
		//scale x and z where z is the seam
		chairSeat.transform.localScale = new Vector3( 0.05f, seatUp.magnitude,seatForward.magnitude);
	}
	
	
}
