using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using UnityEngine;

public class SubMenu : MonoBehaviour {

	public MainMenu menu;

	public TransformTweenBehaviour[] cubeTweens;

	public bool subMenuOpen = false;

	public void ControlSubMenu() {

		if (!subMenuOpen) {
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
			foreach(TransformTweenBehaviour t in cubeTweens) {
				t.PlayBackward();
			}
			subMenuOpen = false;
		}


	}

}
