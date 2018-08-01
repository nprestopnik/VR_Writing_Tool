# Menu Overview
The hand menu consolidates the various tasks the user can perform while using Play|Write. 
There are buttons for each category of actions that reveal throwable cubes that activate the given feature.
As of right now, the menu is in need of an overhaul. There are parts of it that are hard-coded, and other parts that are more dynamic,
so it could use a re-working of the system to make it cleaner and more unified. As it is, here are the most notable menu scripts.

## MainMenu.cs and SubMenu.cs - Activation
These scripts are responsible for activating/deactivating the appropritate parts of the menu.

## ThrowMenuCube.cs and ActivateMenuCubeFunction.cs - cube functionality
The former controls the grasping and throwing of the cubes in coordination with the other menu cubes.
The latter activates the cubes functionality and returns the cube to its place.

## handMenuController.cs (confusingly named)
This script contains methods that some of the cubes call when activated. It doesn't actually control the menu itself.

## MenuHandedness.cs - positioning of menu objects
This script takes each piece of the menu calculates a bunch of positions to place the menu parts (including buttons, cubes, and their tweens)
correctly according to the handedness of the user. Note that the cubes are positioned by the "visible" transform of their tweens. 
How this works could use a lot of improvement.

## CreateWeatherMoodCubes.cs - for the weather/mood submenus
This is the more dynamic part of the menu. Rather than giving the cubes directly to the menu handedness script to start, the presets
from the current scene's Environment Manager are grabbed to generage cubes when each scene is loaded. 
Again, this could use work so the menu system is more cohesive and less messy.
