/*
Tracking Cases
Purpose: currently it is to contain notes about the different cases we can use for tracking vive things
	until we decide what we need/want to do for the system
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCases : MonoBehaviour {

	/*
	this script is just notes for right now!

	combinations of things that can be turned on:
		- equipment should be turned on before entering play mode. doing so after makes things weird
			- and right now it really needs to be set up for one thing

	case 1: two controllers and one tracker
		- camera rig controller manager - left and right as normal left and right controllers, both active
		- object being tracked has tracked object script
		- proceed as normal pretty much

	case 2: one controller and one tracker
		- camera rig controller manager 
			- right: the game object (w/ tracked object script) that will be tracked by the controller
			- left: the game object (w/ tracked object script) that will be tracked by the tracker
		- make sure all controller-type objects in the game (the normal controller objects and the like) are set inactive in the scene
		- the object tracked by the tracker might/will have to be manually set active from the Start method
		- a weird thing with this
			- if you don't have the desk tracker / other thing in the right order (left/right) you can reset the desk's index
				but the other object is inactive and you have to activate it and set its index to whatever the desk's was
				but it goes on if the second controller is turned on... interesting...

	case 3: no controllers and one tracker
		- set the tracker tracked object to the right controller in the controller manager
			- you can just throw an empty object in for the left
		- make sure other controller things are inactive
		- if controllers get turned on at runtime (for whatever reason) use Reset Index function on Tracker Index to put the desk back on the tracker
			- but at that point you can only get the leap marshmallows and not the regular controller models (if they're all active)??
			- setting the original controller objects to the manager and the right indices doesn't seem to help (during runtime)

		- another possibility (maybe?) - if you just leave the normal controllers in the controller manager and turn on the puck
			it thinks that the puck is the right controller but if you deactivate the controller models I'm not sure it matters? it still tracks
	
	use controller model activator script to activate/deactivate vive/leap controllers and models if/when you want them on

	 */
}
