using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEngine.SceneManagement;


public class SaveSystem : MonoBehaviour {

    public static SaveSystem instance;
    public Save currentSave;

    void Awake()
    {
        instance = this; 

        print("Testing for save at: " + Application.persistentDataPath + "/testSave.save");
        if(File.Exists(Application.persistentDataPath + "/testSave.save"))
        {
            //Load save
            currentSave = loadSaveWithName("testSave");
        } else
        {
            //Create new save
            createNewSave("testSave");
        }

        //DEBUG: Lists the names of all the available saves
        Save[] saves = listSaves();
        foreach(Save s in saves) {
            print(s.name + " | " + s.path);
        }
    }

	void Start () {
		//TEMPORARY: Loads the last scene that was saved
        //SceneManager.LoadSceneAsync(currentSave.currentRoomID, LoadSceneMode.Additive);

        //Sets the hallway goal scene to the current save's last open scene
        Hallway.instance.setGoalScene(currentSave.currentRoomID);
	}
	
	void Update () {
		
	}

    public void deleteSave(string name)
    {
        print("DELETING: Testing for save at: " + Application.persistentDataPath + "/" + name + ".save");
        if (File.Exists(Application.persistentDataPath + "/" + name + ".save"))
        {
            //Save exists
            File.Delete(Application.persistentDataPath + "/" + name + ".save");
        }
        else
        {
            //Save does not exist
        }
    }

    public void createNewSave(string name)
    {
        //Create new save and set it to the current one
        Save newSave = new Save(name);
        currentSave = newSave;
        saveCurrentSave();
    }

    public void saveCurrentSave()
    {
        print("SAVING: Testing for save at: " + Application.persistentDataPath + "/" + currentSave.name + ".save");
        if (File.Exists(Application.persistentDataPath + "/" + currentSave.name + ".save"))
        {
            //Save already exists
        }
        else
        {
            //Save does not already exist
        }

        //Converts the save into JSON and saves it to a file
        StreamWriter output = new StreamWriter(Application.persistentDataPath + "/" + currentSave.name + ".save");
        string file = JsonUtility.ToJson(currentSave);
        output.Write(file);
        output.Close();
    }

    public Save loadSaveWithName(string name)
    {
        print("LOADING: Testing for save at: " + Application.persistentDataPath + "/" + name + ".save");
        if (File.Exists(Application.persistentDataPath + "/" + name + ".save"))
        {
            //Save already exists
            //Loads the JSON from a file and converts it into a save
            StreamReader input = new StreamReader(Application.persistentDataPath + "/" + name + ".save");
            Save loadedSave = JsonUtility.FromJson<Save>(input.ReadToEnd());
            input.Close();
            return loadedSave;
        }
        else
        {
            //Save does not already exist
            print("Loading a non-existant save");
            return null;
        }

    }

    public Save[] listSaves()
    {
        //Gets an array of all save files in the directory
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] files = info.GetFiles("*.save");

        //Loads each save and adds them to a List (Probably not the most efficient)
        List<Save> saves = new List<Save>();

        for(int i = 0; i < files.Length; i++)
        {
            string name = files[i].Name;
            print("LOADING FOR LIST: Testing for save at: " + Application.persistentDataPath + "/" + name);
            if (File.Exists(Application.persistentDataPath + "/" + name))
            {
                //Save already exists
                //Loads the JSON from a file and converts it into a save
                StreamReader input = new StreamReader(Application.persistentDataPath + "/" + name);
                Save loadedSave = JsonUtility.FromJson<Save>(input.ReadToEnd());
                loadedSave.path = Application.persistentDataPath + "/" + name;
                input.Close();
                saves.Add(loadedSave);
            }
            else
            {
                //Save does not already exist
                print("Loading a non-existant save");
            }
        }

        return saves.ToArray();
    }

    public void setCurrentSave(Save newSave) {
        currentSave = newSave;
    }

    public Save getCurrentSave() {
        return currentSave;
    }
}
