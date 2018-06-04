using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour {

	Leap.Unity.Interaction.InteractionSlider slider;

	bool isProximity = false;
	bool isFinger = false;

	DrawLineManagerEvents dlme;

	void Awake (){
		slider = GetComponent<Leap.Unity.Interaction.InteractionSlider>();
		dlme = GetComponent<DrawLineManagerEvents>();
	}
	
	public void proximityEnter() {
		print("proximity");
		isProximity = true;
	}

	public void proximityExit() {
		print("end proximity");
		isProximity = false;
	}

	public void fingerEnter() {
		print("finger");
		isFinger = true;
	}

	public void fingerExit() {
		print("end finger");
		isFinger = false;
	}

	public void updateDrawPosition() {
		//slider.tr = slider.primaryHoveringControllerPoint;
	}

	void Update() {


		if(isFinger && isProximity) {
			print("DRAWING");
		} 

	}


	
}
