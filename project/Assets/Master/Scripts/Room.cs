using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Purpose: Data structure for storing room data in project saves */
[System.Serializable]
public class Room {

	public string name; //Name of the room (Practically useless)
	public int sceneID; //Build ID for the scenery
    public Color color; //Color for identification

	[SerializeField]
	Feature[] features; //All the features in the room

	public Room(string name, int sceneID, Color color) {
		this.name = name;
		this.sceneID = sceneID;
        this.color = color;
        features = new Feature[0];
	}

    //Adds a feature to the room
	public void addFeature(Feature newFeature) {
        Feature[] temp = new Feature[features.Length + 1];
        for(int i = 0 ; i < features.Length; i++) {
            temp[i] = features[i];
        }
        temp[features.Length] = newFeature;
        features = temp;
    }

    //Deletes a feature from the room by feature index
    public void deleteFeature(int index) {
        List<Feature> tempList = new List<Feature>(features);
        tempList.RemoveAt(index);
        features = tempList.ToArray();
    }

    //Deletes a feature from the room by object
    public void deleteFeature(Feature feature) {
        List<Feature> tempList = new List<Feature>(features);
        tempList.Remove(feature);
        features = tempList.ToArray();
    }

    //Accessor
    public Feature[] getFeaturesArray() {
        return features;
    }
}
