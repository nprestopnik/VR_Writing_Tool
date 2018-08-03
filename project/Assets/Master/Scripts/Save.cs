using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose: Data structure for how projects are saved. Anything in here will be saved and can be read later */
[System.Serializable]
public class Save {

    //Temporary Save Path. Updated when saves are listed
    public string path;
    //Identification
    public string name;
    //Last room that the player was in
    public int currentRoomIndex;


    [SerializeField]
    Room[] rooms; //Array of rooms in the project


    public Save(string name)
    {
        this.name = name;
        currentRoomIndex = 0;
        rooms = new Room[]{new Room("Null World", 5, Color.black), new Room("Treehouse", 4, Color.green), new Room("Office", 6, Color.blue), new Room("World 1", 1, Color.yellow), new Room("World 2", 2, Color.magenta)};
    }

    //Adds a room to the array
    public void addRoom(Room newRoom) {
        Room[] temp = new Room[rooms.Length + 1];
        for(int i = 0 ; i < rooms.Length; i++) {
            temp[i] = rooms[i];
        }
        temp[rooms.Length] = newRoom;
        rooms = temp;
    }

    //Deletes a room from the array
    public void deleteRoom(int index) {
        List<Room> tempList = new List<Room>(rooms);
        tempList.RemoveAt(index);
        rooms = tempList.ToArray();
    }

    //Accessor
    public Room[] getRoomsArray() {
        return rooms;
    }

    
}
