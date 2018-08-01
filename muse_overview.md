# Muse Overview  
The Muse is the assistant that helps guide the player and inform them about actions. 
  
## `MuseManager.cs`: The Brain  
This is a singleton file that can be accessed from any file during runtime using `MuseManager.instance`. This is the brain of the Muse and is used to enter tasks.  
  
All Muse tasks rely on optional argument callbacks that allow the programmer to pass in functions to be called when the task has been completed. These tasks can be called with or without using the callbacks. For example:  
```
MuseManager.instance.museGuide.EnterMuse(); 
MuseManager.instance.Pause(2f, ()=> NavigateToPoint(hallwayPoint.position, 
  ()=> MuseManager.instance.museText.SetText("Go through the hallway\nto the selected room!", 
  ()=> GetToHallway())));
```  
In this block of code the enter muse task is called immediately. Then the muse is given a pause task at the same time for two seconds with a callback of a set text task. This means that after the Muse has paused for two seconds it will execute the set text task. This set text task also has a callback of the get to hallway task. When the Muse has finished setting the text it will execute the get to hallway method.  
  
## Important Methods:  
#### `MuseManager.instance.MuseNavigation.NavigateToPoint(Vector3 target, Action completedEvent = null)` - This method uses the NavMesh to navigate the muse to a Vector3 in world space. The completed event is called when the muse reaches the end destination. The Muse will also wait for the player to stay near it, so there is a chance that the Muse never reaches it's destination.  
#### `MuseManager.instance.Pause(float seconds, Action completedEvent = null)` - This method is used to delay a callback by a variable amount of seconds. Usually used with the `EnterMuse` method.  
#### `MuseManager.instance.MuseText.SetText(string text, Action completedEvent = null)` - This method changes the text that is displayed on the front of the muse. This method will execute in less than a frame so it can be called before another Muse task.  
#### `MuseManager.instance.MuseText.ClearText(Action completedEvent = null)` - This method is the same as the method above but automatically gets rid of the text. (Identical to passing an empty string into `SetText`)  
#### `MuseManager.instance.GuideToPoint.GuideTo(Transform targetPoint, Action completedEvent = null)` - This method will give the Muse a target transform to move smoothly towards. The callback is executed when the Muse reaches the target. It will continue to follow the target until it is given another one or the target is set to null.  
#### `MuseManager.instance.GuideToPoint.EnterMuse(Action completedEvent = null)` - This method will cause the muse to enter from the predetermined entry point and fly to the front of the player's face. The callback is executed when it reaches the front of the face so it is suggested to pair this method with a `Pause` task.  
#### `MuseManager.instance.GuideToPoint.ExitMuse(Action completedEvent = null)` - This method will cause the muse to exit from wherever it is and go back to a hidden spot behind the player's head. Usually used when the Muse has finished it's task.
