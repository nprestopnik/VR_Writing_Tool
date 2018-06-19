using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnterFromDirection{
	above,below,left,right
}

public class MusePointManager : MonoBehaviour {

	public EnterFromDirection entryDirection;
	public Transform abovePoint;
	public Transform belowPoint;
	public Transform leftPoint;
	public Transform rightPoint;

	public Transform startPoint;

}
