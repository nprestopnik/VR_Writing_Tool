using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnterFromDirection{
	above,below,left,right
}

public class MuseAppear : MonoBehaviour {

	public EnterFromDirection entryDirection;
	public Transform abovePoint;
	public Transform belowPoint;
	public Transform leftPoint;
	public Transform rightPoint;

	public Transform startPoint;

	private GuideToPoint guide;

	void Start() {
		guide = GetComponent<GuideToPoint>();
	}

	void Update() {

	}

	public void EnterMuse() {
		var entry = transform.position;
		switch((int)entryDirection) {
			//above
			case 0:
				entry = abovePoint.position;
				break;
			//below
			case 1:
				entry = belowPoint.position;
				break;
			//left
			case 2:
				entry = leftPoint.position;
				break;
			//right
			case 3:
				entry = rightPoint.position;
				break;
		}

		guide.guiding = true;
		guide.activation = true;
		guide.target = startPoint;
	}

	public void ExitMuse() {
		guide.guiding = true;
		guide.activation = true;

		switch((int)entryDirection) {
			//above
			case 0:
				guide.target = abovePoint;
				break;
			//below
			case 1:
				guide.target = belowPoint;
				break;
			//left
			case 2:
				guide.target = leftPoint;
				break;
			//right
			case 3:
				guide.target = rightPoint;
				break;
		}
	}
}
