using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelSystem : MonoBehaviour {

	public static TravelSystem instance;

	public Room currentRoom;
    public Room goalRoom;
	public int goalRoomIndex=0;
	int goalSceneID = 3;

	void Awake() {
		instance = this;
	}

	void Start () {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	void Update () {
		
	}

	public void fastTravelToScene(int index) {
		setGoalScene(index);
		loadGoalScene();
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        setGoalScene(goalRoomIndex);
    }

	void OnApplicationQuit() {
		//Do stuff
		print("QUITING");
		SaveSystem.instance.saveCurrentSave();
	}

	public bool setGoalScene(int index)
    {
        if(SaveSystem.instance.getCurrentSave() != null) {
            if(testSetGoalScene(index)) {
                goalRoomIndex = index;
                goalRoom = SaveSystem.instance.getCurrentSave().getRoomsArray()[goalRoomIndex];
                goalSceneID = goalRoom.sceneID;
                ControllerMenu.instance.loadRooms();
                return true;
            }
        }
        
        return false;
    }

	public void loadGoalScene() {
		 //Swaps scenes
		//Tests for scenery being the same in each room 
		//Problematic because scene does not reset
		if(goalSceneID != SceneManager.GetActiveScene().buildIndex) {
			SceneManager.LoadSceneAsync(goalSceneID, LoadSceneMode.Additive);
			Scene activeScene = SceneManager.GetActiveScene();
			SceneManager.UnloadSceneAsync(activeScene.buildIndex);  
		} else {
			setGoalScene(goalRoomIndex);
		}
		

		//Updates the save data and then saves it
		currentRoom = SaveSystem.instance.getCurrentSave().getRoomsArray()[goalRoomIndex];
		int temp = SaveSystem.instance.getCurrentSave().currentRoomIndex;
		SaveSystem.instance.getCurrentSave().currentRoomIndex = goalRoomIndex;
		goalRoomIndex = temp;
		// setGoalScene(goalRoomIndex);
		
		SaveSystem.instance.saveCurrentSave();
	}

	public bool testSetGoalScene(int index) {
        //return SaveSystem.instance.getCurrentSave().getRoomsArray()[index].sceneID != SceneManager.GetActiveScene().buildIndex;
        //FIX THIS
        //Selecting a room with the same buildIndex (Background/Scenery) does not work.
        return !(SaveSystem.instance.getCurrentSave().getRoomsArray()[index].name.Equals(currentRoom.name));
    }
}
