using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelSystem : MonoBehaviour {

	public static TravelSystem instance;
	public Texture[] sceneryIcons;

	public Room currentRoom;
    public Room goalRoom;
	public int goalRoomIndex=0;
	int goalSceneID = 3;

	public GameObject whiteboardPrefab;

	void Awake() {
		instance = this;
	}

	void Start () {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	void Update () {
		if(SaveSystem.instance.getCurrentSave() == null) {
 
		} else {
			MuseManager.instance.museNavigator.destBlockMesh.material.color = goalRoom.color;
			MuseManager.instance.museNavigator.destIconMesh.material.SetTexture("_MainTex", sceneryIcons[goalRoom.sceneID]);
		}
	}

	public void fastTravelToRoom(int index) {
		setGoalScene(index);
		loadGoalScene();
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        setGoalScene(goalRoomIndex);
		//Load features
		Feature[] features = SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].getFeaturesArray();

		foreach(Feature f in features) {
			//print("Creating feature: " + f.name);
			if(f is WhiteboardData) {
				//print("Creating Whiteboard: " + ((WhiteboardData)f).text);
				//Create Whiteboard
				Whiteboard whiteboard = ((GameObject)Instantiate(whiteboardPrefab, transform.position, transform.rotation)).GetComponentInChildren<Whiteboard>();
				WhiteboardData data = (WhiteboardData)f;
				whiteboard.loadData(data);
				//Move Whitboard to scene
				SceneManager.MoveGameObjectToScene(whiteboard.transform.root.gameObject, scene);
			}
		}

		SaveSystem.instance.saveCurrentSave();
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
                //ControllerMenu.instance.loadRooms();
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
		
	}

	public bool testSetGoalScene(int index) {
        //return SaveSystem.instance.getCurrentSave().getRoomsArray()[index].sceneID != SceneManager.GetActiveScene().buildIndex;
        //FIX THIS
        //Selecting a room with the same buildIndex (Background/Scenery) does not work.
        return !(SaveSystem.instance.getCurrentSave().getRoomsArray()[index].name.Equals(currentRoom.name));
    }
}
