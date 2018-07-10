using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room {

	public string name;
	public int sceneID;
    public Color color;

	[SerializeField]
	Feature[] features;

	public Room(string name, int sceneID, Color color) {
		this.name = name;
		this.sceneID = sceneID;
        this.color = color;
        features = new Feature[0];
	}

	public void addFeature(Feature newFeature) {
        Feature[] temp = new Feature[features.Length + 1];
        for(int i = 0 ; i < features.Length; i++) {
            temp[i] = features[i];
        }
        temp[features.Length] = newFeature;
        features = temp;
    }

    public void deleteFeature(int index) {
        List<Feature> tempList = new List<Feature>(features);
        tempList.RemoveAt(index);
        features = tempList.ToArray();
    }

    public Feature[] getFeaturesArray() {
        return features;
    }
}
