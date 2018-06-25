using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActivator : MonoBehaviour {

    [Tooltip("Set this to the buildIndex of the scene that it belongs in")]
    public int buildIndex;

	void Start () {
        //Sets the correct scene as active for lighting and scene loading to work.
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
        //ControllerMenu.instance.loadRooms();
        TravelSystem.instance.setGoalScene(TravelSystem.instance.goalRoomIndex);
    }
	
}
