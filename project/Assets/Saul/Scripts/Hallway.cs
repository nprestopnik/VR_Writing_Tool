using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hallway : MonoBehaviour {

    public static Hallway instance;

    public int goalSceneID = 2;

    public Room goalRoom;
    public int goalRoomIndex=0;

    [Tooltip("This point should be dead center in a perfectly symmetrical hallway")]
    public Transform rotatePoint;

    void Awake() {
        instance = this;
    }

	void Start () {
        //TEMPORARY: Sets goal scene to the loaded scene
        //goalSceneID = (saveSystem.currentSave.currentRoomID == 2) ? 1 : 2;
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
            int temp = SaveSystem.instance.getCurrentSave().currentRoomIndex;
            SaveSystem.instance.getCurrentSave().currentRoomIndex = goalRoomIndex;
            goalRoomIndex = temp;
            setGoalScene(goalRoomIndex);
            SaveSystem.instance.saveCurrentSave();

            
            //TEMPORARY: Sets the goal scene to the other scene
            //goalSceneID = (goalSceneID == 2) ? 1 : 2;
            
        }
    }

    //Sets the goal scene ID
    public void setGoalScene(int index)
    {
        //if(index != goalRoomIndex) {
            goalRoomIndex = index;
            goalRoom = SaveSystem.instance.getCurrentSave().getRoomsArray()[goalRoomIndex];
            goalSceneID = goalRoom.sceneID;
        //}
    }
}
