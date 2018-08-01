# Idea Board Overview  
Idea Boards are interactive boards that can display text or images and can be drawn on. These boards are persistant and are saved to the project file. In code they are referred to as whiteboards and are split into three major scripts.  
  
### `Whiteboard.cs` - The brain of the Idea Board  
This script is responsible for all the interactions of the Idea Board. It contains all the methods for each button interaction as well as drawing interactions. It also updates the data so that it can be saved in the future.  
  
### `WhiteboardData.cs` - Data Structure for saving Idea Boards  
This class is not a monobehavior so it cannot be added to a gameobject. However it can be constructed normally and serialized without a problem. It extends the Feature class and is saved as a feature to the project file.
  
### `WhiteboardContainer.cs` - A simple container for `WhiteboardData`  
This allows for any gameobject to be able to hold `WhiteboardData`. It bypasses the restriction of the data not being a monobehavior.
