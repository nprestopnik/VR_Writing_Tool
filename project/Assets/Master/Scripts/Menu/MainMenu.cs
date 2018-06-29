using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public TransformTweenBehaviour[] menuButtonTweens;

	public SubMenu[] subMenus;

	public bool isActive;

	
	public void ActivateMenu() {
		isActive = true;
		foreach(TransformTweenBehaviour t in menuButtonTweens) {
			t.PlayForward();
		}
	}

	public void DeactivateMenu() {
		isActive = false;
		foreach(SubMenu s in subMenus) {
			if (s.subMenuOpen) {
				s.ControlSubMenu();
			}
		}
		
		foreach(TransformTweenBehaviour t in menuButtonTweens) {
			t.PlayBackward();
		}
	}

}
