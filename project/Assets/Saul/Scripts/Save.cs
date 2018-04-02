using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Save {

    //Temporary Save Path. Updated when saves are listed
    public string path;
    //Identification
    public string name;
    //Last scene that the player was in
    public int currentRoomID;

    public Save(string name)
    {
        this.name = name;
        currentRoomID = 1;
    }

    
}
