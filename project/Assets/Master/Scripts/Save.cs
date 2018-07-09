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


    [SerializeField]
    Room[] rooms;


    public Save(string name)
    {
        this.name = name;
        currentRoomIndex = 0;
        rooms = new Room[]{new Room("CRUD Room", 5), new Room("Base Room", 1), new Room("Secondary Room", 2), new Room("Tree House", 4)};
    }

    public void addRoom(Room newRoom) {
        Room[] temp = new Room[rooms.Length + 1];
        for(int i = 0 ; i < rooms.Length; i++) {
            temp[i] = rooms[i];
        }
        temp[rooms.Length] = newRoom;
        rooms = temp;
    }

    public void deleteRoom(int index) {
        List<Room> tempList = new List<Room>(rooms);
        tempList.RemoveAt(index);
        rooms = tempList.ToArray();
    }

    public Room[] getRoomsArray() {
        return rooms;
    }

    
}
