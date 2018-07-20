using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCreateMenu : MonoBehaviour {

	public RawImage iconImage;
	public MeshRenderer colorMesh;
	public RoomCubeContainer rcc;
	Leap.Unity.Interaction.AnchorableBehaviour cubeAnchorBeh;
	public Leap.Unity.Interaction.Anchor cubeAnchor;



	public SceneIcon[] icons;
	public Color[] colors;

	private int iconIndex = 0;
	private int colorIndex = 0;

	void OnEnable () {
		cubeAnchorBeh = rcc.GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>();
		updateDisplay();
	}

	public void setIconIndexAdditive(int amount) {
		iconIndex += amount;
		if(iconIndex >= icons.Length) {
			iconIndex -= icons.Length;
		}
		if(iconIndex < 0) {
			iconIndex += icons.Length;
		}
		updateDisplay();
	}

	public void setColorIndexAdditive(int amount) {
		colorIndex += amount;
		if(colorIndex >= colors.Length) {
			colorIndex -= colors.Length;
		}
		if(colorIndex < 0) {
			colorIndex += colors.Length;
		}
		updateDisplay();
	}

	public void updateDisplay() {
		iconImage.texture = icons[iconIndex].sceneIcon;
		colorMesh.material.color = colors[colorIndex];

		rcc.room = new Room("CREATED FROM ROOM CREATOR", icons[iconIndex].sceneID, colors[colorIndex]);

		rcc.blockMesh.material.color = colors[colorIndex];
		rcc.iconMesh.material.SetTexture("_MainTex", icons[iconIndex].sceneIcon);
	}

	public void RoomCubeTrigger() {
		cubeAnchorBeh.transform.position = cubeAnchor.transform.position;
		cubeAnchorBeh.transform.rotation = cubeAnchor.transform.rotation;
		cubeAnchorBeh.anchor = cubeAnchor;

		SaveSystem.instance.getCurrentSave().addRoom(rcc.room);
		SaveSystem.instance.saveCurrentSave();

		gameObject.SetActive(false);
	}

}

[System.Serializable]
public struct SceneIcon {
	public int sceneID;
	public Texture sceneIcon;
}
