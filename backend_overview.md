# Backend Overview
The backend in this project refers to the Save System and Travel System.  
Both of these are hidden from the user but play a vital role in how the software works.  
  
## The Save System (`SaveSystem.cs`)  
This file is responsible for reading and writing project files. It is a singleton file and can be accessed from any file during runtime using `SaveSystem.instance`.  
  
#### The most common methods used will be:  
`SaveSystem.instance.saveCurrentSave()` - Writes the current project to the predetermined path  
`SaveSystem.instance.getCurrentSave()` - Returns the current project that is loaded
  
## The Travel System (`TravelSystem.cs`)  
This file is responsible for managing scenes when the player moves from room to room. It is also used to create and features that were saved in a room when it is loaded. It is a singleton file and can be accessed from any other file during runtime using `TravelSystem.instance`.  
  
#### The most common methods used will be:  
`TravelSystem.instance.setGoalScene(int roomIndex)` - Used to set the goal room for the hallway. The parameter takes in the index of the room in the Save's room array, NOT the index in the build settings
