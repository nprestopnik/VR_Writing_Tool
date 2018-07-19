/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2017.                                 *
 * Leap Motion proprietary and  confidential.                                 *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/
 //I basically took a few functions from the leap menu example and modified them to work with our system

using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionThrownObject : MonoBehaviour {

	public static PositionThrownObject instance;

	public float placementHeightFromCamera = -0.35f; //how much below the camera should the thing spawn
	public float minPlacementDistance = 0.7F; //how far away should the thing spawn, minimum
    public float maxPlacementDistance = 0.71F; //how far away should the thing spawn, maximum

	[HideInInspector]
	public Vector3 cubeInitPosition; //the position of the cube when it activates
	[HideInInspector]
	public Vector3 cubeInitVelocity; //the velocity of the cube when it activates

	void Awake() {
		instance = this;
	}

	/*
	determine a good position for an object to spawn based on the position and velocity of the thrown cube that spawns it
	cubeInitPosiiton and cubeInitVelocity must be set first in order for this to work; they are currently set from the cube activation script 
	*/
	public Vector3 DeterminePosition() {
     
		Vector3 userEyePosition = Camera.main.transform.position;
		Quaternion userEyeRotation = Camera.main.transform.rotation;
	 
		// Push velocity away from the camera if necessary.
    	Vector3 towardsCamera = (userEyePosition - cubeInitPosition).normalized;
    	float towardsCameraness = Mathf.Clamp01(Vector3.Dot(towardsCamera, cubeInitVelocity.normalized));
    	cubeInitVelocity = cubeInitVelocity + Vector3.Lerp(Vector3.zero, -towardsCamera * 2.00F, towardsCameraness);

    	// Calculate velocity direction on the XZ plane.
    	Vector3 groundPlaneVelocity = Vector3.ProjectOnPlane(cubeInitVelocity, Vector3.up);
    	float groundPlaneDirectedness = groundPlaneVelocity.magnitude.Map(0.003F, 0.40F, 0F, 1F);
    	Vector3 groundPlaneDirection = groundPlaneVelocity.normalized;

    	// Calculate camera "forward" direction on the XZ plane.
    	Vector3 cameraGroundPlaneForward = Vector3.ProjectOnPlane(userEyeRotation * Vector3.forward, Vector3.up);
    	float cameraGroundPlaneDirectedness = cameraGroundPlaneForward.magnitude.Map(0.001F, 0.01F, 0F, 1F);
    	Vector3 alternateCameraDirection = (userEyeRotation * Vector3.forward).y > 0F ? userEyeRotation * Vector3.down : userEyeRotation * Vector3.up;
    	cameraGroundPlaneForward = Vector3.Slerp(alternateCameraDirection, cameraGroundPlaneForward, cameraGroundPlaneDirectedness);
    	cameraGroundPlaneForward = cameraGroundPlaneForward.normalized;

    	// Calculate a placement direction based on the camera and throw directions on the XZ plane.
    	Vector3 placementDirection = Vector3.Slerp(cameraGroundPlaneForward, groundPlaneDirection, groundPlaneDirectedness);

    	// Calculate a placement position along the placement direction between min and max placement distances.
    	Vector3 placementPosition = userEyePosition + placementDirection * Mathf.Lerp(minPlacementDistance, maxPlacementDistance, (groundPlaneDirectedness * cubeInitVelocity.magnitude).Map(0F, 1.50F, 0F, 1F)); 

    	// Don't move far if the initial velocity is small.
    	// float overallDirectedness = cubeInitVelocity.magnitude.Map(0.00F, 3.00F, 0F, 1F);
    	// placementPosition = Vector3.Lerp(cubeInitPosition, placementPosition, overallDirectedness * overallDirectedness);
      
    	// Enforce placement height.
    	placementPosition.y = userEyePosition.y + placementHeightFromCamera;

    	// Enforce minimum placement away from user.
    	Vector2 cameraXZ = new Vector2(userEyePosition.x, userEyePosition.z);
    	Vector2 stationXZ = new Vector2(placementPosition.x, placementPosition.z);
    	float placementDist = Vector2.Distance(cameraXZ, stationXZ);
    	if (placementDist < minPlacementDistance) {
    		float distanceLeft = (minPlacementDistance - placementDist);
    		Vector2 xzDisplacement = (stationXZ - cameraXZ).normalized * distanceLeft;
    		placementPosition += new Vector3(xzDisplacement[0], 0F, xzDisplacement[1]);
    	}

    	return placementPosition;
    }

	/*
	determine a good rotation for the thing!
	position must be calculated first and sent along to this method
	 */
	public Quaternion DetermineRotation(Vector3 objectPosition) {
		Vector3 userEyePos = Camera.main.transform.position;

    	Vector3 toCamera = userEyePos - objectPosition;
    	toCamera.y = 0F;
    	Quaternion placementRotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);

    	return placementRotation;
    }


}
