using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEngine.SceneManagement;
using FullSerializer;


/*Purpose: Manages saving and loading of project files as well as system config file */
public class SaveSystem : MonoBehaviour {

    public static SaveSystem instance; //Singleton
    Save currentSave; //Currently loaded save

    ConfigData config; //Currently loaded config file

    void Awake()
    {
        instance = this; 

        //DEBUG: Lists the names of all the available saves
        Save[] saves = listSaves();
        foreach(Save s in saves) {
            print(s.name + " | " + s.path);
        }


        config = loadConfigData(); //Automatically tries to load config data
    }

    //When the application is closed
    void OnApplicationQuit() {
		//Do stuff
		print("QUITING");
		saveCurrentSave(); //Save the current project
        SaveConfigData(); //Save the config file
	}

    //Deletes file at a path
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

    //Creates a new save at a path
    public Save createNewSave(string path)
    {
        //Create new save and set it to the current one
        print(path.Length + " " + (path.Length - 5 - 1) + " " + ((Application.persistentDataPath + "/").Length - 1));
        string name = path.Substring((Application.persistentDataPath + "/").Length, path.Length - 5 - (Application.persistentDataPath + "/").Length ); //Grabs the name from the path
        Save newSave = new Save(name); //Creates a new project
        currentSave = newSave; //Sets it active
        
        currentSave.path = path; //Updates the project's path
        saveCurrentSave(); //Saves it
        return currentSave; //Returns it
    }

    //Saves the current project
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
        //USE FSSERIALIZER TO SAVE ALL FILES (Allows for saving of more advanced data types)
        fsSerializer _serializer = new fsSerializer();
        fsData data;
        _serializer.TrySerialize(typeof(Save), currentSave, out data).AssertSuccessWithoutWarnings(); //Converts file
        StreamWriter output = new StreamWriter(currentSave.path); //Opens writer
        output.Write(fsJsonPrinter.CompressedJson(data)); //Writes to the file
        output.Close(); //Closes it
        return (currentSave.path); //Returns the path
    }

    //Load a save by the name (Honestly don't use this. Use the path one instead)
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
            print(loadedSave.getRoomsArray()[2].getFeaturesArray()[0].GetType());
            return loadedSave;
        }
        else
        {
            //Save does not already exist
            print("Loading a non-existant save");
            return null;
        }

    }

    //Loads a save by its path
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

            //deserialize the data
            fsSerializer _serializer = new fsSerializer();
            object deserialized = null;
            _serializer.TryDeserialize(data, typeof(Save), ref deserialized).AssertSuccessWithoutWarnings();
            Save loadedSave = (Save)deserialized;
            //print(loadedSave.getRoomsArray()[2].getFeaturesArray()[0].GetType());
            return loadedSave;
        }
        else
        {
            //Save does not already exist
            print("Loading a non-existant save");
            return null;
        }

    }

    //Returns a list of all the projects it can find in the usual directory
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
                // StreamReader input = new StreamReader(Application.persistentDataPath + "/" + name);
                // Save loadedSave = JsonUtility.FromJson<Save>(input.ReadToEnd());
                
                Save loadedSave = loadSaveWithPath(Application.persistentDataPath + "/" + name);
                loadedSave.path = Application.persistentDataPath + "/" + name;
                //input.Close();
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


    //Sets the current project and loads the first room in it to the hallway
    public void setCurrentSave(Save newSave) {
        currentSave = newSave;
        if(currentSave != null)
        TravelSystem.instance.fastTravelToRoom(0);
    }

    //Accessor
    public Save getCurrentSave() {
        return currentSave;
    }

    //Writes the config file to a JSON file
    public void SaveConfigData() {
        fsSerializer _serializer = new fsSerializer();
        fsData data;
        _serializer.TrySerialize(typeof(ConfigData), config, out data).AssertSuccessWithoutWarnings();
        StreamWriter output = new StreamWriter(Application.persistentDataPath + "/config.config");
        output.Write(fsJsonPrinter.CompressedJson(data));
        output.Close();
    }

    //Returns config data from the file saved, if the file is not found it creates a new file
    ConfigData loadConfigData() {
        ConfigData loadedConfig;
        if(!File.Exists(Application.persistentDataPath + "/config.config")) { //Cannot find the config file
            loadedConfig = new ConfigData(); //Creates new one
        } else {
            StreamReader input = new StreamReader(Application.persistentDataPath + "/config.config"); //Reads in config file
            string serializedState = input.ReadToEnd();
            input.Close();
            fsData data = fsJsonParser.Parse(serializedState);

            // deserialize the data
            fsSerializer _serializer = new fsSerializer();
            object deserialized = null;
            _serializer.TryDeserialize(data, typeof(ConfigData), ref deserialized).AssertSuccessWithoutWarnings();
            loadedConfig = (ConfigData)deserialized;
            //print(loadedSave.getRoomsArray()[2].getFeaturesArray()[0].GetType());
        }
        return loadedConfig;
    }

    //Accessor
    public ConfigData getConfigData() {
        return config;
    }
}
