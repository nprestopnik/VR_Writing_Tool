using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Control Hallway doors through triggers and procedural animations */
public class DoorTrigger : MonoBehaviour {


    public AnimationCurve doorEasing; //Curve for door easing.

    public GameObject leftDoor;
    public GameObject rightDoor;

    public float doorAnimationTime; //Time it takes to complete the animation
    public int animationSteps = 20; //"Frames" in the animation

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && SaveSystem.instance.getCurrentSave() != null && !(TravelSystem.instance.currentRoom.sceneID == TravelSystem.instance.goalRoom.sceneID)) //If the player is near the hallway and there is a loaded save
        {
            StartCoroutine(doorAnimation(leftDoor, false, false));
            StartCoroutine(doorAnimation(rightDoor, false, true));
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" && SaveSystem.instance.getCurrentSave() != null) //If the player leaves the trigger and there is a loaded save
        {
            StartCoroutine(doorAnimation(leftDoor, true, false));
            StartCoroutine(doorAnimation(rightDoor, true, true));
        }
    }

    IEnumerator doorAnimation(GameObject door, bool reverse, bool mirror) {
        int steps = animationSteps;
        for(int i = 0; i <= steps; i++) {
            float stamp = reverse ? (1 - (i / (steps * 1f))) : (i / (steps * 1f)); //if reversed, the animation goes backwards
            float goalDegree = mirror ? -90f : 90f; //if mirrored the goal degree is 180 degrees more
            door.transform.localRotation = Quaternion.Euler(0,doorEasing.Evaluate(stamp) * goalDegree ,0); //Updates y rotation
            yield return new WaitForSeconds(doorAnimationTime / steps); //Waits a "Frame"
        }
    }
}
