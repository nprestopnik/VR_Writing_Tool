using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public GameObject door;

    //TEMPORARY: Quick and dirty door programming. 

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            door.SetActive(false);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            door.SetActive(true);
        }
    }
}
