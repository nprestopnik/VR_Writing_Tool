using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*Purpose: Manages the room creation menu */
public class RoomCreateMenu : MonoBehaviour {

	public RawImage iconImage; //Display for the icon that is selected
	public MeshRenderer colorMesh; //Display for the color that is selected
	public RoomCubeContainer rcc; //Contains the data for the created room
	Leap.Unity.Interaction.AnchorableBehaviour cubeAnchorBeh;
	public Leap.Unity.Interaction.Anchor cubeAnchor;



	public SceneIcon[] icons; //List of Icons 
	public Color[] colors; //List of colors

	private int iconIndex = 0; //Current icon index that is selected
	private int colorIndex = 0; //Current color index that is selected


	//When the menu is enabled update the display
	void OnEnable () {
		cubeAnchorBeh = rcc.GetComponent<Leap.Unity.Interaction.AnchorableBehaviour>();
		updateDisplay();
	}


	//Additively sets the icon index
	public void setIconIndexAdditive(int amount) {
		iconIndex += amount;
		//Ensures no index out of range
		if(iconIndex >= icons.Length) {
			iconIndex -= icons.Length;
		}
		if(iconIndex < 0) {
			iconIndex += icons.Length;
		}
		updateDisplay(); //Updates the display
	}

	//Additively sets the color index
	public void setColorIndexAdditive(int amount) {
		colorIndex += amount;
		//Ensures no index out of range
		if(colorIndex >= colors.Length) {
			colorIndex -= colors.Length;
		}
		if(colorIndex < 0) {
			colorIndex += colors.Length;
		}
		updateDisplay(); //Updates display
	}

	//Used to update the display menu
	public void updateDisplay() {
		iconImage.texture = icons[iconIndex].sceneIcon; //Sets the display to mirror the selected icon
		colorMesh.material.color = colors[colorIndex]; //Sets the display to mirror the selected color

		rcc.room = new Room("CREATED FROM ROOM CREATOR", icons[iconIndex].sceneID, colors[colorIndex]); //Creates the room witht the scenery and color

		rcc.blockMesh.material.color = colors[colorIndex]; //Sets the cube's color to mirror the selected color
		rcc.iconMesh.material.SetTexture("_MainTex", icons[iconIndex].sceneIcon); //Sets the cube's icon to mirror the selected icon
	}


	//Used to finalize room creation
	public void RoomCubeTrigger() {
		//Brings cube back to anchor
		cubeAnchorBeh.transform.position = cubeAnchor.transform.position;
		cubeAnchorBeh.transform.rotation = cubeAnchor.transform.rotation;
		cubeAnchorBeh.anchor = cubeAnchor;
		cubeAnchorBeh.TryAttach();

		SaveSystem.instance.getCurrentSave().addRoom(rcc.room); //Adds the room to the current project
		SaveSystem.instance.saveCurrentSave(); //Saves the updated project
		TravelSystem.instance.setGoalScene(SaveSystem.instance.getCurrentSave().getRoomsArray().Length -1); //Sets the hallway goal to be the newly created room

		MuseManager.instance.museNavigator.NavigateToHallway(); //Calls the muse to help with hallway navigation

		gameObject.SetActive(false); //Deactivates menu to hide it
	}

}

/*Purpose: Creates a data type that links scene ids and icons */
[System.Serializable]
public struct SceneIcon {
	public int sceneID;
	public Texture sceneIcon;
}
