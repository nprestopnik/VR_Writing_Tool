# Creating a Scenery

Several things have to be done in order to set up a scenery to work in the PLAY | WRITE system.   
  
### Setting Up the Scenery
1. Line up the Scenery with the Hallway Scene
2. Place the Door Facade
3. Bake Lighting with Light Probes
4. Bake Navigation Mesh
5. Add and setup an Environment Manager in the scene


### Adding the Scenery to the Loading System
1. Add the Scene to the Built Scenes (File -> Build Settings... -> Add Open Scenes)  
2. Add the "[NECESSARY] SetSceneActive" Prefab to the scene and set the Build index in the Scene Activator script
to be the same as the Scenery's build index (The number next to the scene in build settings)  
3. Load the Hallway Scene and find the Travel System Script on the [Camera Rig]. Add an Icon in the Scenery Icons Array that is at the same index as the build index of the Scenery  
4. Find the "Room Create Menu" that is a child of [Camera Rig]. Add an icon to the Icon array. 

## Now you're all set! The Scenery should now be fully integrated into the system.
