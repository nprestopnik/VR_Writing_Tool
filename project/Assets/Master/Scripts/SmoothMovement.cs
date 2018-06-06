using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement : MonoBehaviour {

    SteamVR_TrackedController controller;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    SteamVR_Camera camera;
    Transform playerRoot;

    public float moveSpeed = 1f;
    public float moveViewSpeed = 8f;

    public MeshRenderer fovQuad;

    public GameObject head;
    public GameObject room;
    float headOffset = 5f;
    float maxStepHeight = 0.3f;

    void Start () {
        playerRoot = transform.parent;
        controller = GetComponent<SteamVR_TrackedController>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        camera = head.GetComponent<SteamVR_Camera>();
        
        
    }
	
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        Vector3 headPosition = camera.head.position;


        RaycastHit hit;
        Vector3 castPosition = new Vector3(headPosition.x, room.transform.position.y + headOffset, headPosition.z);

        if (controller.padPressed)
        {
            if (Physics.SphereCast(castPosition, 0.15f, Vector3.down, out hit, 50f, 1<<0))
            {
                if(Mathf.Abs(hit.point.y - room.transform.position.y) < maxStepHeight)
                {
                    Vector2 touchpadAxis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

                    Vector3 dir = new Vector3(transform.forward.x, 0, transform.forward.z);
                    playerRoot.position = Vector3.Lerp(playerRoot.position, playerRoot.position + dir, Time.deltaTime * moveSpeed);
                    fovQuad.materials[0].color = Color.Lerp(fovQuad.materials[0].color, new Color(0, 0, 0, 1f), Time.deltaTime * moveViewSpeed);

                    playerRoot.position = new Vector3(playerRoot.position.x, hit.point.y, playerRoot.position.z);
                }
            }
        }
        else
        {
            fovQuad.materials[0].color = Color.Lerp(fovQuad.materials[0].color, new Color(0, 0, 0, 0f), Time.deltaTime * moveViewSpeed);

        }

        Debug.DrawRay(castPosition, Vector3.down * 50f, Color.magenta);

    }
}
