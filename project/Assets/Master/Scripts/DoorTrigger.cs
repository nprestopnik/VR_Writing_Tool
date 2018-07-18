using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {


    public AnimationCurve doorEasing;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public float doorAnimationTime;
    public int animationSteps = 20;


    void Update() {

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && SaveSystem.instance.getCurrentSave() != null)
        {
            StartCoroutine(doorAnimation(leftDoor, false, false));
            StartCoroutine(doorAnimation(rightDoor, false, true));
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" && SaveSystem.instance.getCurrentSave() != null)
        {
            StartCoroutine(doorAnimation(leftDoor, true, false));
            StartCoroutine(doorAnimation(rightDoor, true, true));
        }
    }

    IEnumerator doorAnimation(GameObject door, bool reverse, bool mirror) {
        int steps = animationSteps;
        for(int i = 0; i <= steps; i++) {
            float stamp = reverse ? (1 - (i / (steps * 1f))) : (i / (steps * 1f));
            float goalDegree = mirror ? -90f : 90f;
            door.transform.localRotation = Quaternion.Euler(0,doorEasing.Evaluate(stamp) * goalDegree ,0);
            yield return new WaitForSeconds(doorAnimationTime / steps);
        }
    }
}
