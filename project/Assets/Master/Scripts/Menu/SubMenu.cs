using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using UnityEngine;

public class SubMenu : MonoBehaviour {

	public TransformTweenBehaviour[] cubeTweens;

	public bool subMenuOpen = false;

	public void ControlSubMenu() {

		if (!subMenuOpen) {
			foreach(TransformTweenBehaviour t in cubeTweens) {
				t.PlayForwardAfterDelay(0f);
				subMenuOpen = true;
			}
		}
		else {
			foreach(TransformTweenBehaviour t in cubeTweens) {
				t.PlayBackwardAfterDelay(0f);
				subMenuOpen = false;
			}
		}


	}

}
