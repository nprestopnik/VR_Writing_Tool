/*
Hand Menu Controller
functions for cubes in the hand menu
cubes handled currently: save, exit, go to location, idea boards, desk
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class handMenuController : MonoBehaviour {

	public GameObject whiteBoardPrefab;
	public DeskManager dm;
	public GameObject RoomsMenu;
	public GameObject RoomsCreateMenu;
	public GameObject RoomsDeleteMenu;
	public Transform head;

	void Start () {
		
	}


	public void save() {
		if(SaveSystem.instance.getCurrentSave() != null)
			SaveSystem.instance.saveCurrentSave();
	}

	public void quit() {
		//Application.Quit();
		SaveSystem.instance.saveCurrentSave();
		SaveSystem.instance.setCurrentSave(null);
		SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
		Scene activeScene = SceneManager.GetActiveScene();
		SceneManager.UnloadSceneAsync(activeScene.buildIndex);
	}

	//open the shelf that has all of the available room cubes on it and position it not terribly
	public void openRoomLoadMenu() {
		if(SaveSystem.instance.getCurrentSave() != null) {
			RoomsMenu.SetActive(true);

			//RoomsMenu.transform.position = PositionThrownObject.instance.DeterminePosition();
			RoomsMenu.transform.position = DetermineVisiblePosition();
			RoomsMenu.transform.rotation = PositionThrownObject.instance.DetermineRotation(RoomsMenu.transform.position);
			//RoomsMenu.transform.position = head.forward + head.transform.position;
		}

	}

	public void openRoomCreateMenu() {
		if(SaveSystem.instance.getCurrentSave() != null) {
			RoomsCreateMenu.SetActive(true);

			RoomsCreateMenu.transform.position = DetermineVisiblePosition(); //PositionThrownObject.instance.DeterminePosition();
			RoomsCreateMenu.transform.rotation = PositionThrownObject.instance.DetermineRotation(RoomsCreateMenu.transform.position);
			//RoomsMenu.transform.position = head.forward + head.transform.position;
		}

	}

	public void openRoomDeleteMenu() {
		if(SaveSystem.instance.getCurrentSave() != null) {
			RoomsDeleteMenu.SetActive(true);

			RoomsDeleteMenu.transform.position = DetermineVisiblePosition();//PositionThrownObject.instance.DeterminePosition();
			RoomsDeleteMenu.transform.rotation = PositionThrownObject.instance.DetermineRotation(RoomsDeleteMenu.transform.position);
			//RoomsMenu.transform.position = head.forward + head.transform.position;
		}

	}

	public void createIdeaBoard() {
		Vector3 pos = DetermineVisiblePosition();

		Whiteboard whiteboard = ((GameObject)Instantiate(whiteBoardPrefab, pos, transform.rotation)).GetComponentInChildren<Whiteboard>();
		WhiteboardData data = new WhiteboardData();
		// whiteboard.transform.root.position = PositionThrownObject.instance.DeterminePosition();
		whiteboard.transform.root.rotation = PositionThrownObject.instance.DetermineRotation(whiteboard.transform.root.position);

		whiteboard.transform.root.Rotate(0, 180, 0);
		data.position = whiteboard.transform.root.position;
		data.rotation = whiteboard.transform.root.rotation;
		//whiteboard.orientRotation();
		whiteboard.loadData(data);
		SaveSystem.instance.getCurrentSave().getRoomsArray()[SaveSystem.instance.getCurrentSave().currentRoomIndex].addFeature(whiteboard.dataContainer.data);
		//whiteboard.orientRotation();
		SaveSystem.instance.saveCurrentSave();
	}

	//determine a position for the object to spawn that is in front of the user
	Vector3 DetermineVisiblePosition() {
		Vector3 pos = (head.forward);// + head.transform.position;
		pos.y = 0;
		pos.Normalize();
		pos /= 1.5f;
		pos += head.transform.position;
		pos.y = head.transform.position.y - 0.5f;
		return pos;
	}


	public void activateDesk() {
		dm.StartDeskTask(true);
	}
}
