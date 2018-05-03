using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hallway : MonoBehaviour {

    public static Hallway instance;

    int goalSceneID = 3;

    public Room goalRoom;
    public Room currentRoom;
    public int goalRoomIndex=0;

    [Tooltip("This point should be dead center in a perfectly symmetrical hallway")]
    public Transform rotatePoint;

    void Awake() {
        instance = this;
    }

	void Start () {
        //TEMPORARY: Sets goal scene to the loaded scene
        //goalSceneID = (saveSystem.currentSave.currentRoomID == 2) ? 1 : 2;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        //Checks if the entering object is the player
        if(col.tag == "Player")
        {        
            //Rotates the player around the centerPoint
            col.transform.root.RotateAround(rotatePoint.position, Vector3.up, 180);
     
            //Swaps scenes
            SceneManager.LoadSceneAsync(goalSceneID, LoadSceneMode.Additive);
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(activeScene.buildIndex);

            //Updates the save data and then saves it
            currentRoom = SaveSystem.instance.getCurrentSave().getRoomsArray()[goalRoomIndex];
            int temp = SaveSystem.instance.getCurrentSave().currentRoomIndex;
            SaveSystem.instance.getCurrentSave().currentRoomIndex = goalRoomIndex;
            goalRoomIndex = temp;
            // setGoalScene(goalRoomIndex);
            
            SaveSystem.instance.saveCurrentSave();

            
            //TEMPORARY: Sets the goal scene to the other scene
            //goalSceneID = (goalSceneID == 2) ? 1 : 2;
            
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        setGoalScene(goalRoomIndex);
    }

    //Sets the goal scene ID
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

    public bool testSetGoalScene(int index) {
        //return SaveSystem.instance.getCurrentSave().getRoomsArray()[index].sceneID != SceneManager.GetActiveScene().buildIndex;
        //FIX THIS
        //Selecting a room with the same buildIndex (Background/Scenery) does not work.
        return !(SaveSystem.instance.getCurrentSave().getRoomsArray()[index].name.Equals(currentRoom.name));
    }
}
