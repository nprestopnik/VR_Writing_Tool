using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Super depricated... Not recommended for use
Purpose: Put one of these on a controller and teleports the camera rig when the touchpad is pressed */
public class TeleportMovement : MonoBehaviour {

    SteamVR_TrackedController controller;
    SteamVR_Camera camera;

    public GameObject pointer;
    public GameObject head;
    public GameObject room;

    public LineRenderer pointLine;

    bool isTeleportPrep = false;
    bool isArcTeleport = false;

    Vector3 teleportPosition;

    void Start () {
        controller = GetComponent<SteamVR_TrackedController>();
        camera = head.GetComponent<SteamVR_Camera>();

        controller.PadClicked += padDown;
        controller.PadUnclicked += padUp;
        controller.MenuButtonClicked += menuUp;
    }
	
	void Update () {
        RaycastHit hit;

        pointLine.gameObject.SetActive(isTeleportPrep);

        //hit = recursiveArcCast(pointer.transform, 0.2f, 0, 2, -9.8f);

        if (isTeleportPrep)
        {
            if(isArcTeleport)
            {
                hit = recursiveArcCast(pointer.transform, 1, 0, 2, -2);
                teleportPosition = hit.point;

                pointLine.positionCount = 2;
                pointLine.useWorldSpace = true;
                pointLine.SetPositions(new[] { transform.position, hit.point});
                
            } else
            {
                if (Physics.Raycast(pointer.transform.position, pointer.transform.forward, out hit, 50f, 1 << 0))
                {
                    if (hit.normal.y > 0.5f)
                    {
                        teleportPosition = hit.point;
                        pointLine.positionCount = 2;
                        pointLine.useWorldSpace = false;
                        pointLine.SetPositions(new[] { Vector3.zero, Vector3.forward * hit.distance });
                    }
                }
            }
        }
	}

    void padDown(object sender, ClickedEventArgs e)
    {
        isTeleportPrep = true;
    }

    void padUp(object sender, ClickedEventArgs e)
    {
        isTeleportPrep = false;
        Vector3 headPosition = camera.head.localPosition;
        Vector3 offset = new Vector3(headPosition.x, 0, headPosition.z);

        room.transform.position = teleportPosition - (transform.root.rotation * offset);
    }

    void menuUp(object sender, ClickedEventArgs e)
    {
        //isArcTeleport = !isArcTeleport;
    }

    //DUMB NOT WORKING GARBAGE
    RaycastHit recursiveArcCast(Transform initialPointer, float stepSize, int currentStep, float initialVelocity, float gravity)
    {
        if(currentStep < 15)
        {
            RaycastHit hit;
            Vector3 position1 = new Vector3(initialPointer.position.x + (initialVelocity * currentStep),
                                            (0.5f * gravity * currentStep * currentStep) + (initialVelocity * currentStep) + initialPointer.position.y,
                                             initialPointer.position.z + (initialVelocity * currentStep));
            currentStep++;
            Vector3 position2 = new Vector3(initialPointer.position.x + (initialVelocity * currentStep),
                                            (0.5f * gravity * currentStep * currentStep) + (initialVelocity * currentStep) + initialPointer.position.y,
                                             initialPointer.position.z + (initialVelocity * currentStep));
            Vector3 dir = position2 - position1;

            pointLine.positionCount = currentStep;
            pointLine.SetPosition(currentStep - 1, position2);
            Debug.DrawLine(position1, position2, Color.red);

            //print(currentStep);

            if (Physics.Raycast(position1, dir, out hit, stepSize, 1 << 0))
            {
                return hit;
            }
            else
            {
                return recursiveArcCast(initialPointer, stepSize, currentStep, initialVelocity, gravity);
            }
        } else
        {
            return new RaycastHit();
        }
        
    }
}
