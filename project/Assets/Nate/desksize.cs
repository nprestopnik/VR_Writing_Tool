using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desksize : MonoBehaviour {

	IDictionary<string, GameObject> deskParts = new Dictionary<string, GameObject>(); //A dictionary told hold all desk parts by name

	//Public Method takes in w, h, d and runs the program to scale the desk
	public void scaleDesk (float width, float height, float depth) {
		
		//Build a dictionary of desk parts
		foreach (Transform part in transform) //In the desk gameObject, loop through every part
		{
			deskParts.Add(part.name, part.gameObject); //Add each part to the dictionary, along with its name
		}
	
		//Scale the desk top and mount in w and d (but keep h at default)
		scalePart(deskParts["top"].transform, w: width, d: depth); //Scale the desk surface (height should remain at default scale)
		scalePart(deskParts["mount"].transform, w: width, d: depth); //Scale the desk mount platform (height should remain at default scale)

		positionPart(deskParts["top"].transform, y: height, yOffSet: 0.01F);
		positionPart(deskParts["mount"].transform, y: height, yOffSet: 0.05F);
		
		//Scale the legs in h, but keep (w and d at default)
		foreach (Transform leg in deskParts["legs"].transform) //In the desk gameObject, loop through every part
		{
			print(leg.localToWorldMatrix.GetScale());
			scalePart(leg, h: height);
			positionPart(leg, y: leg.localScale.y / 2);
		}
		/*		
		
		scalePart(deskParts["front_left"], h: height);
		scalePart(deskParts["front_right"], h: height);
		scalePart(deskParts["rear_left"], h: height);
		scalePart(deskParts["rear_right"], h: height);

		//Position the legs
		positionPart(deskParts["front_left"], h: (height / 2) - 0.92F);

		//Position the desk top and mount on top of the legs
		positionPart(deskParts["top"], h: height/2);
		positionPart(deskParts["mount"], h: height/2);

		print(height/2);
		*/
	}

	//Scales any part of the desk in x, y, z
	private void scalePart (Transform part, float w = 1.0F, float h = 1.0F, float d = 1.0F) {
		part.localScale = new Vector3(part.localScale.x * w, part.localScale.y * h, part.localScale.z * d);
	}

	//Positions the desk top and mount relative to the legs
	private void positionPart(Transform part, float x = 0.0F, float y = 0.0F, float z = 0.0F, float yOffSet = 0.0F) {
		part.position = new Vector3(x, y - yOffSet, z);
	}

	// Use this for initialization
	void Start () {

		

		scaleDesk(2.3F, 2F, 1.0F); //Scale the desk

		
		//Scale the leg heights
		//scalePart(deskParts["front_left"], 1.0F, 1.0F, 1.0F);
		//scalePart(deskParts["front_right"], 1.0F, 1.0F, 1.0F);
		//scalePart(deskParts["rear_left"], 1.0F, 1.0F, 1.0F);
		//scalePart(deskParts["rear_right"], 1.0F, 1.0F, 1.0F);




		


		//partsArray[4].transform.localScale = new Vector3(deskWidth * 100.0F, transform.localScale.y, deskDepth * 100.0F); //Scale the desk surface in x, z
		//partsArray[5].transform.localScale = new Vector3(deskWidth, 1.0F, deskHeight); //Scale the mount in x, z

		//Scale each leg in y
		//for (int i = 0; i <= 3; i++) {
		//	partsArray[i].transform.localScale = new Vector3(1.0F, 1.0F, deskHeight);
		//}

		//Position the legs

		//transform.localScale += new Vector3(deskWidth, deskDepth, deskHeight);		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
