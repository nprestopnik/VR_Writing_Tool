using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desksize : MonoBehaviour {

	IDictionary<string, GameObject> deskParts = new Dictionary<string, GameObject>(); //A dictionary told hold all desk parts by name

	public float width; //Holds the desk width in meters (from calibrator)
	public float height; //Holds the desk height in meters (from calibrator)
	public float depth; //Holds the desk depth in meters (from calibrator)
	private float prevTopHeight = 0.0F; //Holds on to the height of desk surfaces to help calculate positions as the desk is built
	private float totalTopHeight = 0.0F; //Accumulator used to hold total desk surface height and size legs
	private const float legSpacing = 0.06F; //The distance between a pair of legs (front and back) in cm (measured from leg center)
	private const float cmConvert = 100.0F; //A constant used to convert from meters to centimeters or vice versa

	//Public setter that can be used to set the desk scale in meters (in unity, the values can be typed in the inspector because they are public)
	public void setScale(float w = 1.0F, float h = 1.0F, float d = 1.0F) {
		width = w;
		height = h;
		depth = d;
	}

	//Public method to populate a dictionary of desk parts, with part names as keys
	public void deskSetup() {
		//Build a dictionary of desk parts
		foreach (Transform part in transform) { //In the desk gameObject, loop through every part
			deskParts.Add(part.name, part.gameObject); //Add each part to the dictionary, along with its name
		}
	}

	//Public method to run all the scaling and positioning operations for each part
	//Because parts have unique requirements, the calls are made individually instead of by looping the dictionary (for now?)
	//Order matters here; some scaling or positioning operations produce data used by subsequent scaling or positioning operations
	public void scaleDesk () {

		//Scale the desk top and mount based on defined width and depth (height will remain the same)
		scaleTop(deskParts["top"].transform);
		scaleTop(deskParts["mount"].transform);

		//Position the desk top and mount relative to each other and the newly defined height (the top of the desk top will be at the defined height)
		positionTop(deskParts["top"].transform);
		positionTop(deskParts["mount"].transform);

		//Scale the four legs of the desk for height, so they are sized fill the space between the bottom of the mount and the floor
		scaleLeg(deskParts["front_left"].transform);
		scaleLeg(deskParts["front_right"].transform);
		scaleLeg(deskParts["rear_left"].transform);
		scaleLeg(deskParts["rear_right"].transform);
		
		//Position the four legs of the desk, so they fill the space between the bottom of the mount and the floor
		//Also position them in width and depth based so they are appropriately positioned underneath the table to support the surface
		positionLeg(deskParts["front_left"].transform, wDir: -1.0F, sidePos: legSpacing);
		positionLeg(deskParts["front_right"].transform, sidePos: legSpacing);
		positionLeg(deskParts["rear_left"].transform, dDir: -1.0F, wDir: -1.0F);
		positionLeg(deskParts["rear_right"].transform, dDir: -1.0F);		
	}

	//A method that will scale the desk top and mount in width and depth (height is not scaled because we want top and mount heights to be the same, no matter the desk scale)
	private void scaleTop (Transform part) {
		part.localScale = new Vector3(part.localScale.x * width, part.localScale.y, part.localScale.z * depth);
	}

	//Positions the desk top and mount based on height. Th prevTopHeight and totalTopHeight vars are used to store information about the positioning and scale of these elements so the legs can scale properly.
	//In this current solution, the top and mount must be scaled before the legs, and the top must be scaled before the mount
	private void positionTop(Transform part) {
		part.localPosition = new Vector3(part.localPosition.x, height - (((part.localScale.y / 2) + prevTopHeight) / cmConvert), part.localPosition.z);
		prevTopHeight = part.localScale.y;
		totalTopHeight += prevTopHeight;
	}

	//Scales the table legs in y dimension (x and z are not scaled). Y scale is accomplished by using previously accumulated totalTopheight value and deducting that from the overall desk height.
	private void scaleLeg (Transform part) {
		part.localScale = new Vector3(part.localScale.x, (height * cmConvert) - totalTopHeight, part.localScale.z);
	}

	//Positions the four legs in x, y, and z. wDir determines if a leg will move in positive or negative X (width), dDir determines same for Z (depth). sidePos is used to space rear legs from their corresponding front leg pairs.
	private void positionLeg(Transform part, float wDir = 1.0F, float dDir = 1.0F, float sidePos = 0.0F) {

		float legX = ((part.localPosition.x + width / 2.0F) - 0.20F + sidePos) * wDir; //Calculate X position
		float legY = (height / 2) - (totalTopHeight / 2) / cmConvert; //Calculate Y position 
		float legZ = ((part.localPosition.z + depth / 2.0F) - 0.40F) * dDir; //Cal;culate Z position
		
		part.localPosition = new Vector3(legX, legY, legZ);
	}

	// Currently, start is only needed to run the three public methods that set the scale, build the dictionary, and run the desk scaler method.
	//These may be called elsewhere in the final implementation
	void OnEnable () {
		//setScale(2.0F, 1.0F, 1.0F);
		deskSetup();
		scaleDesk();
		foreach(Transform t in transform) {
			t.localPosition += new Vector3(0, -1 * height, depth / 2f);	
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
