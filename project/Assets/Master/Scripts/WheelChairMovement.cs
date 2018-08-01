using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Super depricated... Not recommended for use
Purpose: Put one of these on each controller and moves the camera rig when grips are pressed */
public class WheelChairMovement : MonoBehaviour {

    SteamVR_TrackedController controller;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    SteamVR_Camera camera;

    bool isWheelchair = false;
    Transform playerRoot;

    public GameObject head;
    public GameObject room;
    float headOffset = 3f;
    float maxStepHeight = 0.3f;


    void Start () {
        controller = GetComponent<SteamVR_TrackedController>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        camera = head.GetComponent<SteamVR_Camera>();

        controller.Gripped += gripDown;
        controller.Ungripped += gripUp;

        playerRoot = transform.root;
    }

    
	
	void FixedUpdate () {

        Vector3 headPosition = camera.head.position;

        RaycastHit hit;
        Vector3 castPosition = new Vector3(headPosition.x, room.transform.position.y + headOffset, headPosition.z);

        if (isWheelchair)
        {
            if (Physics.Raycast(castPosition, Vector3.down, out hit, 50f, 1 << 0))
            {
                if (Mathf.Abs(hit.point.y - room.transform.position.y) < maxStepHeight)
                {
                    device = SteamVR_Controller.Input((int)trackedObj.index);
                    Vector3 controllerVel = -1 * new Vector3(device.velocity.x, 0, device.velocity.z);
                    controllerVel = transform.root.rotation * controllerVel;
                    
                    playerRoot.position += controllerVel * Time.deltaTime;

                    playerRoot.position = new Vector3(playerRoot.position.x, hit.point.y, playerRoot.position.z);
                }
            }
        }

        Debug.DrawRay(castPosition, Vector3.down * 50f, Color.magenta);

    }

    void gripDown(object sender, ClickedEventArgs e)
    {
        isWheelchair = true;
    }

    void gripUp(object sender, ClickedEventArgs e)
    {
        isWheelchair = false;
    }
}
