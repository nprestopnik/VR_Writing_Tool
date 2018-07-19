﻿/*
Base Station Orientation
Purpose: make sure the lighthouse models are standing upright when they are activated
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOrientation : MonoBehaviour {

	void Update () {
		transform.rotation = Quaternion.LookRotation(Vector3.down);
	}

}
