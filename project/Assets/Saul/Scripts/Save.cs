using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Save {

    //Temporary Save Path. Updated when saves are listed
    public string path;
    //Identification
    public string name;
    //Last room that the player was in
    public int currentRoomIndex;

    public List<Room> rooms;

    public Save(string name)
    {
        this.name = name;
        currentRoomIndex = 0;
        rooms = new List<Room>(new Room[]{new Room("Base Room", 1)});
    }

    
}
