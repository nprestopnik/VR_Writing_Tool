using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Purpose: VERY IMPORTANT
One of these needs to be in every scenery */
public class SceneActivator : MonoBehaviour {

    [Tooltip("Set this to the buildIndex of the scene that it belongs in")]
    public int buildIndex;

	void Start () {
        //Sets the correct scene as active for lighting and scene loading to work.
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));

        TravelSystem.instance.setGoalScene(TravelSystem.instance.goalRoomIndex);
    }
	
}
