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
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        //Checks if the entering object is the player
        if(col.tag == "Player")
        {        
            //Rotates the player around the centerPoint
            col.transform.root.RotateAround(rotatePoint.position, Vector3.up, 180);
     
            TravelSystem.instance.loadGoalScene();
            
        }
    }
}
