using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEngine.SceneManagement;
using FullSerializer;


public class SaveSystem : MonoBehaviour {

    public static SaveSystem instance;
    Save currentSave;

    void Awake()
    {
        instance = this; 

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
        //Hallway.instance.setGoalScene(currentSave.currentRoomIndex);
	}
	
	void Update () {
		
	}

    public void deleteSave(string path)
    {
        print("DELETING: Testing for save at: " + path);
        if (File.Exists(path))
        {
            //Save exists
            File.Delete(path);
        }
        else
        {
            //Save does not exist
        }
    }

    public Save createNewSave(string path)
    {
        //Create new save and set it to the current one
        print(path.Length + " " + (path.Length - 5 - 1) + " " + ((Application.persistentDataPath + "/").Length - 1));
        string name = path.Substring((Application.persistentDataPath + "/").Length, path.Length - 5 - (Application.persistentDataPath + "/").Length );
        Save newSave = new Save(name);
        currentSave = newSave;
        
        currentSave.path = path;
        saveCurrentSave();
        return currentSave;
    }

    public string saveCurrentSave()
    {
        print("SAVING: Testing for save at: " + currentSave.path);
        if (File.Exists(currentSave.path))
        {
            //Save already exists
        }
        else
        {
            //Save does not already exist
        }

        //Converts the save into JSON and saves it to a file

        fsSerializer _serializer = new fsSerializer();
        fsData data;
        _serializer.TrySerialize(typeof(Save), currentSave, out data).AssertSuccessWithoutWarnings();
        StreamWriter output = new StreamWriter(currentSave.path);
        output.Write(fsJsonPrinter.CompressedJson(data));
        output.Close();
        return (currentSave.path);
    }

    public Save loadSaveWithName(string name)
    {
        print("LOADING: Testing for save at: " + Application.persistentDataPath + "/" + name + ".save");
        if (File.Exists(Application.persistentDataPath + "/" + name + ".save"))
        {
            //Save already exists
            //Loads the JSON from a file and converts it into a save

            StreamReader input = new StreamReader(Application.persistentDataPath + "/" + name + ".save");
            string serializedState = input.ReadToEnd();
            input.Close();
            fsData data = fsJsonParser.Parse(serializedState);

            // step 2: deserialize the data
            fsSerializer _serializer = new fsSerializer();
            object deserialized = null;
            _serializer.TryDeserialize(data, typeof(Save), ref deserialized).AssertSuccessWithoutWarnings();
            Save loadedSave = (Save)deserialized;
            return loadedSave;
        }
        else
        {
            //Save does not already exist
            print("Loading a non-existant save");
            return null;
        }

    }

    public Save loadSaveWithPath(string path)
    {
        print("LOADING: Testing for save at: " + path);
        if (File.Exists(path))
        {
            //Save already exists
            //Loads the JSON from a file and converts it into a save

            StreamReader input = new StreamReader(path);
            string serializedState = input.ReadToEnd();
            input.Close();
            fsData data = fsJsonParser.Parse(serializedState);

            // step 2: deserialize the data
            fsSerializer _serializer = new fsSerializer();
            object deserialized = null;
            _serializer.TryDeserialize(data, typeof(Save), ref deserialized).AssertSuccessWithoutWarnings();
            Save loadedSave = (Save)deserialized;
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
