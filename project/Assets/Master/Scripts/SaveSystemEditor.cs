using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveSystem))]
public class SaveSystemEditor : Editor {

	bool creatingSave = false;
	bool deletingSave = false;
	bool loadingSave = false;

	string saveName = "testSave";
	int goalRoom = 2;

	public override void OnInspectorGUI() {
		SaveSystem myTarget = (SaveSystem)target;
		DrawDefaultInspector();
		
		GUILayout.Space(10f);

		//Create save button
		if(creatingSave) {
			saveName = EditorGUILayout.TextField("Save Name", saveName);
			if(GUILayout.Button("Create")) {
				myTarget.createNewSave(Application.persistentDataPath + "/" + saveName + ".save");
				creatingSave = false;
			}
			if(GUILayout.Button("Cancel")) {
				creatingSave = false;
			}
		} else {
			if(GUILayout.Button("Create Save")) {
				creatingSave = true;
			}
		}

		GUILayout.Space(15f);
		//Deleting saves button
		if(deletingSave) {
			GUILayout.Label("Saves:");
			Save[] saves = myTarget.listSaves();
			foreach(Save s in saves) {
				if(GUILayout.Button(s.name)) {
					myTarget.deleteSave(s.path);
					deletingSave = false;
				}
			}
			GUILayout.Space(6f);
			if(GUILayout.Button("Cancel")) {
				deletingSave = false;
			}
		} else {
			if(GUILayout.Button("Delete Save")) {
				deletingSave = true;
			}
		}

		GUILayout.Space(15f);
		//Loading save button
		if(loadingSave) {
			GUILayout.Label("Saves:");
			Save[] saves = myTarget.listSaves();
			foreach(Save s in saves) {
				if(GUILayout.Button(s.name)) {
					myTarget.setCurrentSave(myTarget.loadSaveWithPath(s.path));
					loadingSave = false;
				}
			}
			GUILayout.Space(6f);
			if(GUILayout.Button("Cancel")) {
				loadingSave = false;
			}
		} else {
			if(GUILayout.Button("Load Save")) {
				loadingSave = true;
			}
		}

		//Displaying save information
		GUILayout.Space(15f);
		if(myTarget.getCurrentSave() != null && Application.isPlaying) {
			GUILayout.Label("Current Save:");
			GUILayout.Label(myTarget.getCurrentSave().name);
			GUILayout.Label(myTarget.getCurrentSave().path);
			GUILayout.Label("Current goal: " + TravelSystem.instance.goalRoomIndex);
			goalRoom = EditorGUILayout.IntField("Current Room Index:", goalRoom);
			if(GUILayout.Button("Update GoalRoomIndex")) {
				TravelSystem.instance.setGoalScene(goalRoom);
			}
			//myTarget.getCurrentSave().currentRoomIndex = EditorGUILayout.IntField("Current Room Index:", myTarget.getCurrentSave().currentRoomIndex);
		}
	}
}
