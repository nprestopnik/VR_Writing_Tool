using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*DEPRICATED Purpose: Used to pick up items with a specific layer
NO LONGER BEING USED */
public class PickUpItem : MonoBehaviour {

    SteamVR_TrackedController controller;
    SteamVR_Controller.Device device;

    public GameObject holder;
    public GameObject capsule;

	void Start () {
        controller = GetComponent<SteamVR_TrackedController>();

        //Binds the methods to the click delegates
        controller.TriggerClicked += triggerClick;
        controller.TriggerUnclicked += triggerUnClick;
    }
	
	void Update () {
        capsule.transform.position = gameObject.transform.position;
        capsule.transform.rotation = gameObject.transform.rotation;
	}

    //When the trigger is pressed the first pickupable item will be set as a child to the controller.
    void triggerClick(object sender, ClickedEventArgs e)
    {
        Collider[] items = Physics.OverlapBox(transform.position, new Vector3(0.06f, 0.06f, 0.06f), transform.rotation, 1 << 8);

        if (items.Length > 0)
        {
            items[0].GetComponent<Rigidbody>().isKinematic = true;
            items[0].transform.SetParent(holder.transform);
        }
    }

    //When the trigger is unclicked all picked up items are released from the controller and sent back to the active scene
    void triggerUnClick(object sender, ClickedEventArgs e)
    {
        device = SteamVR_Controller.Input((int)controller.controllerIndex);

        foreach (Transform t in holder.transform)
        {
            Rigidbody rig = t.GetComponent<Rigidbody>();
            rig.isKinematic = false;
            t.SetParent(null);
        
            rig.velocity = transform.root.rotation * device.velocity;
            rig.angularVelocity = transform.root.rotation * device.angularVelocity;
            SceneManager.MoveGameObjectToScene(t.gameObject, SceneManager.GetActiveScene());
        }        
    }
}
