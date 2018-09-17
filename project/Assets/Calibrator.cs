using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


//Enum to designate stages of calibration. Also links stages to indecies of panels
enum CalibrationStage { DesignateController = 0, CalibrateDesk = 1, CalibrateChair = 2 }

/*Purpose: manage the calibration menus and input of the chair and desk */
public class Calibrator : MonoBehaviour
{

    CalibrationStage currentStage = CalibrationStage.DesignateController; //The current stage of calibration

    public GameObject calibrationPanel;

    public desksize deskResizer; //Script that resizes the desk
    public ChairSizer chairResizer; //Script that resizes the chair

    //Prefabs for chair creation
    public GameObject backPivotPrefab;
    public GameObject seatPivotPrefab;

    //Each panel describing what to do for each stage of calibration
    //Index of panel matches calibration stage number
    public GameObject[] stagePanels;

    public CanvasRenderer[] deskPointVisuals;
    public CanvasRenderer[] chairPointVisuals;

    public SteamVR_TrackedController rightController;
    public SteamVR_TrackedController leftController;

    SteamVR_TrackedController calibrationController; //The controller used for calibration. Set in the first stage
    SteamVR_TrackedController chairController; //The controller used as the chair. 
    public SteamVR_TrackedObject deskTracker;
    List<Vector3> deskCalibrationPoints; //Calibration points in local space to the tracker
    List<Vector3> chairCalibrationPoints; //Calibration points in local space to the chair controller


    void Start()
    {
        //Defaults controller for when the application is loaded with pre-existing calibration
        chairController = leftController; //Defaults controller to left
        calibrationController = rightController;

        setupChair(SaveSystem.instance.getConfigData().chairCalibrationPoints); //Auto calibrates chair
                                                                                //Sets all children in the chair to not active
        foreach (Transform t in chairController.transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        //Defaults calibration to beginning stage
        currentStage = CalibrationStage.DesignateController;

        //Adds delegate methods to controller
        leftController.Gripped += gripsClicked;
        rightController.Gripped += gripsClicked;

    }

    void Update()
    {
        //Turns all panels off at the beginning of the frame (Inefficient? Probably)
        foreach (GameObject g in stagePanels)
        {
            g.SetActive(false);
        }

        //Activates the panel that is at the same index of the currentStage
        stagePanels[(int)currentStage].SetActive(true);

        //Designate Controller Stage
        //Determines which controller is the calibration controller
        if (currentStage == CalibrationStage.DesignateController)
        {
            if (rightController.triggerPressed)
            {
                calibrationController = rightController;
                chairController = leftController;
                deskCalibrationPoints = new List<Vector3>();
                chairCalibrationPoints = new List<Vector3>();
                calibrationController.TriggerClicked += triggerClicked; //Adds trigger clicked delegate to calibration controller
                currentStage = CalibrationStage.CalibrateDesk; //Changes stage to the next one
            }
            else if (leftController.triggerPressed)
            {
                calibrationController = leftController;
                chairController = rightController;
                deskCalibrationPoints = new List<Vector3>();
                chairCalibrationPoints = new List<Vector3>();
                calibrationController.TriggerClicked += triggerClicked;
                currentStage = CalibrationStage.CalibrateDesk;
            }
        }

        if (currentStage == CalibrationStage.CalibrateDesk)
        {
            for (int i = 0; i < deskCalibrationPoints.Count; i++)
            {
                deskPointVisuals[i].SetColor(Color.green);
            }
        }
        else if (currentStage == CalibrationStage.CalibrateChair)
        {
            for (int i = 0; i < chairCalibrationPoints.Count; i++)
            {
                chairPointVisuals[i].SetColor(Color.green);
            }
        }
        else
        {
            foreach (CanvasRenderer c in chairPointVisuals)
            {
                c.SetColor(Color.white);
            }
            foreach (CanvasRenderer c in deskPointVisuals)
            {
                c.SetColor(Color.white);
            }
        }
    }


    //Used to swap chairs if the chair controller is wrong
    //Triggered by clicking the grips on the chair you want to be the calibration controller
    void gripsClicked(object sender, ClickedEventArgs e)
    {
        if ((SteamVR_TrackedController)sender == rightController)
        {
            calibrationController = rightController;
            chairController.GetComponent<ChairSizer>().destroyChair(); //Destroys the chair on the old controller
            chairController = leftController;
            setupChair(SaveSystem.instance.getConfigData().chairCalibrationPoints); //Creates a new chair on the new controller

        }
        else if ((SteamVR_TrackedController)sender == leftController)
        {
            calibrationController = leftController;
            chairController.GetComponent<ChairSizer>().destroyChair();
            chairController = rightController;
            setupChair(SaveSystem.instance.getConfigData().chairCalibrationPoints);
        }
    }


    //Trigger clicked event on the controllers. Used to get calibration points
    void triggerClicked(object sender, ClickedEventArgs e)
    {
        if (currentStage == CalibrationStage.DesignateController)
        {

        }
        else if (currentStage == CalibrationStage.CalibrateDesk)
        { //Desk Calibration Phase
            if (deskCalibrationPoints.Count <= 2)
            { //Adds the calibration point position local to the tracker
                deskCalibrationPoints.Add(deskTracker.transform.InverseTransformPoint(calibrationController.transform.Find("Calibration Point").position));
                //print(deskCalibrationPoints[deskCalibrationPoints.Count - 1].x);
                //print(deskCalibrationPoints[deskCalibrationPoints.Count - 1].y);
            }

            if (deskCalibrationPoints.Count == 2)
            { //When there are 2 calibration points (Top right and bottom left corners of the desk)
              //math stuff
                float width, height, depth;
                height = deskTracker.transform.localPosition.y; //Height is based on the height from the calibrated ground
                width = deskCalibrationPoints[1].x - deskCalibrationPoints[0].x; //Width is calculated using the x values from the points
                depth = deskCalibrationPoints[0].y - deskCalibrationPoints[1].y; //Depth is calculated using the y values from the points
                print(width);
                print(depth);

                deskResizer.setScale(width, height, depth); //Calibrates desk using the calculated points

                currentStage = CalibrationStage.CalibrateChair; //Updates current stage
            }
        }
        else if (currentStage == CalibrationStage.CalibrateChair)
        { //Chair calibration stage
            if (chairCalibrationPoints.Count <= 4)
            { //Adds the calibration point local to the chair controller
                chairCalibrationPoints.Add(chairController.transform.InverseTransformPoint(calibrationController.transform.Find("Calibration Point").position));
            }

            if (chairCalibrationPoints.Count == 4)
            { //When the calibrator has 4 points
              //math stuff
                setupChair(chairCalibrationPoints); //Calibrates chair with points

                //Calibration is now done
                //Sends data to the config file
                SaveSystem.instance.getConfigData().chairCalibrationPoints = chairCalibrationPoints;
                SaveSystem.instance.getConfigData().deskCalibrationPoints = deskCalibrationPoints;
                SaveSystem.instance.SaveConfigData(); //Saves config file


                //Sets all children in the chair to not active
                foreach (Transform t in chairController.transform)
                {
                    t.gameObject.SetActive(false);
                }
                //Resets the stage and deactivates the calibrator panel
                currentStage = CalibrationStage.DesignateController;
                calibrationPanel.SetActive(false);
            }
        }
    }


    //Used to calibrate the chair
    void setupChair(List<Vector3> points)
    {
        chairResizer = chairController.gameObject.AddComponent<ChairSizer>(); //Adds the chairSizer script to the chair controller
                                                                              //Updates prefabs in the chairSizer
        chairResizer.backPivotPrefab = backPivotPrefab;
        chairResizer.seatPivotPrefab = seatPivotPrefab;
        //Calibrates chair with the points
        chairResizer.setCalibrationPoints(points);
    }

    //Accessor
    public SteamVR_TrackedController getChairController()
    {
        return chairController;
    }

    public SteamVR_TrackedController getCalibrationController()
    {
        return calibrationController;
    }
}
