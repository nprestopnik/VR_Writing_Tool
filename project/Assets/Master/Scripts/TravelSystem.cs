using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Purpose: Manages scene loading and hallway control
	ONLY USE THIS SCRIPT TO LOAD SCENES */
public class TravelSystem : MonoBehaviour {

	public static TravelSystem instance; //Singleton
	public Texture[] sceneryIcons; //Array of scenery icons for destination cube

	public Room currentRoom; //Current loaded room
    public Room goalRoom; //Goal room
	public int goalRoomIndex=0; //Goal room index in the current project
	int goalSceneID = 0; //Goal scene id in the build settings

	public GameObject whiteboardPrefab; //Prefab of the whiteboard for loading

	void Awake() {
		instance = this; //Sets singleton
		SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
	}

	void Start () {
		SceneManager.sceneLoaded += OnSceneLoaded; //Adds onSceneLoaded to SceneManager Event delegate
	}
	
	void Update () {
		if(SaveSystem.instance.getCurrentSave() == null) {
 
		} else {
			//Updates destination cube
			MuseManager.instance.museNavigator.destBlockMesh.material.color = goalRoom.color;
			MuseManager.instance.museNavigator.destIconMesh.material.SetTexture("_MainTex", sceneryIcons[goalRoom.sceneID]);
		}
	}

	//Used to fast travel to a room without using the hallway.
	//VERY UNSTABLE. Will most likely load in a gamebreaking location
	public void fastTravelToRoom(int index) {
		setGoalScene(index);
		loadGoalScene();
	}

	//Called everytime a new scene is loaded
	public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

		if(SaveSystem.instance.getCurrentSave() != null) {
			setGoalScene(goalRoomIndex);
			//Load features
			Feature[] features = SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].getFeaturesArray(); //Gets the array of features for the loaded room

			foreach(Feature f in features) { //Creating new feature in the loaded room. Add to this when a new feature is added
				//print("Creating feature: " + f.name);
				if(f is WhiteboardData) { //If the feature being added is a whiteboard
					//print("Creating Whiteboard: " + ((WhiteboardData)f).text);
					//Create Whiteboard
					Whiteboard whiteboard = ((GameObject)Instantiate(whiteboardPrefab, transform.position, transform.rotation)).GetComponentInChildren<Whiteboard>();
					WhiteboardData data = (WhiteboardData)f;
					whiteboard.loadData(data);
					//Move Whitboard to scene
					SceneManager.MoveGameObjectToScene(whiteboard.transform.root.gameObject, scene);
				}
			}

			SaveSystem.instance.saveCurrentSave(); //Saves the current project
		}
    }

	//Used to set the goal scene of the hallway. Takes in the room index NOT THE SCENE ID
	//Returns true if it worked
	public bool setGoalScene(int index)
    {
        if(SaveSystem.instance.getCurrentSave() != null) {
            if(testSetGoalScene(index)) {
                goalRoomIndex = index;
                goalRoom = SaveSystem.instance.getCurrentSave().getRoomsArray()[goalRoomIndex];
                goalSceneID = goalRoom.sceneID;
                //ControllerMenu.instance.loadRooms();
                return true;
            }
        }
        
        return false;
    }

	//Used when scenes are actually being swapped
	public void loadGoalScene() {
		//Swaps scenes
		//Tests for scenery being the same in each room 
		//Problematic because scene does not reset
		if(goalSceneID != SceneManager.GetActiveScene().buildIndex) {
			SceneManager.LoadSceneAsync(goalSceneID, LoadSceneMode.Additive); //Loads the new scene additively
			Scene activeScene = SceneManager.GetActiveScene(); //Gets the active scene
			SceneManager.UnloadSceneAsync(activeScene.buildIndex);  //Unloads the active scene (When the new scene loads it becomes active)
		} else {
			setGoalScene(goalRoomIndex);
		}
		

		//Updates the save data and then saves it
		currentRoom = SaveSystem.instance.getCurrentSave().getRoomsArray()[goalRoomIndex];

		int temp = SaveSystem.instance.getCurrentSave().currentRoomIndex; //Updates the project
		SaveSystem.instance.getCurrentSave().currentRoomIndex = goalRoomIndex;
		goalRoomIndex = temp;
		
	}

	//Used to test if the new scene is viable. Currently always true, but might need to be changed with duplicates of the same scenery with different features
	public bool testSetGoalScene(int index) {
        //return SaveSystem.instance.getCurrentSave().getRoomsArray()[index].sceneID != SceneManager.GetActiveScene().buildIndex;
        //FIX THIS
        //Selecting a room with the same buildIndex (Background/Scenery) does not work.
        return true;
		//return !(SaveSystem.instance.getCurrentSave().getRoomsArray()[index].name.Equals(currentRoom.name));
    }
}
