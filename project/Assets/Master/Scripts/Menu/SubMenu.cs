/*
Sub Menu Controller
Purpose: controls the activation and deactivation of a submenu when a button is pressed
Attach the Control Sub Menu method to the appropriate button script
 */

using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using UnityEngine;

public class SubMenu : MonoBehaviour {

	public MainMenu menu; //the menu!

	public TransformTweenBehaviour[] cubeTweens; //the tweens for the cubes in this submenu

	public bool subMenuOpen = false; //whether or not the submenu is currently open
	

	//activates or deactivates the submenu based on whether or not it is currenly active
	public void ControlSubMenu() {

		if (!subMenuOpen) {
			//if this submenu is currently not open, close all other submenus and open this one
			foreach(SubMenu s in menu.subMenus) {
				if (s.subMenuOpen) {
					s.ControlSubMenu();
				}
			}
			foreach(TransformTweenBehaviour t in cubeTweens) {
				t.PlayForward();
			}
			subMenuOpen = true;
		}
		else {
			//if the submenu is already open, close it
			foreach(TransformTweenBehaviour t in cubeTweens) {
				t.PlayBackward();
			}
			subMenuOpen = false;
		}


	}

}
