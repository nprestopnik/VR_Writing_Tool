using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Animation;
using UnityEngine;

public class MoodContainer : MonoBehaviour {

	public LightingPreset preset;

	Transform cubeParent;
	[HideInInspector]
	public MeshRenderer blockMesh;
	[HideInInspector]
	public MeshRenderer iconMesh;

	Transform tweenParent;
	[HideInInspector] 
	public TransformTweenBehaviour tween;
	[HideInInspector] 
	public Transform hiddenTween;

	//this should probably be in start but it was behaving oddly???? i don't know 
	public void Setup() {
		cubeParent = transform.Find("Cube");
		blockMesh = cubeParent.Find("block mesh").gameObject.GetComponent<MeshRenderer>();
		iconMesh = cubeParent.Find("icon mesh").gameObject.GetComponent<MeshRenderer>();

		tweenParent = transform.parent.Find("Mood Tween");
		tween = tweenParent.GetComponent<TransformTweenBehaviour>();
		hiddenTween = tweenParent.Find("Hidden");
	}

	public void SetMood() {
		WeatherSystemManager.instance.environment.SetLighting(preset);
	}
}
