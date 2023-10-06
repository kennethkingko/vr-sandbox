using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twist2ActionComponent : BaseActionComponent
{
    // Set in Inspector
    public float stopMoveBuffer = 0;
    public AudioSource audioData;
    // requirement = required angle //set as negative if counterclockwise
    
    // currentProgress = deltaAngle + deltaAngleBuffer
    // totalProgress = pastProgress + currentProgress
    GameObject interactingObject;    
    float deltaAngleBuffer;
    float interactingObjAngleInitial;
    float pastProgress;
    bool checkTime = false;
    float timeStart;
    public float reqTime;
   
    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
        currentProgress = 0;
        totalProgress = 0;
    }


    public override void Update()
    {
        if (!isCompleted && interactingObject != null) CheckIfCompleted();
        if (checkTime)
        {
            float timeEnd = Time.time;
            float diff = timeEnd-timeStart;
            Debug.Log("time: " + diff);
        }
    }


    public override void OnEntry(GameObject go)
    {
        if (go != null)
        {
            interactingObject = go;
            Vector3 pos = go.transform.position;
            Quaternion rot = go.transform.rotation;
            interactingObjAngleInitial = rot.eulerAngles.z;
            //Debug.Log("Entry transform: " + pos + " " + rot);        
            deltaAngleBuffer = 0;
            pastProgress = totalProgress;
            audioData.Play(0);
        }
    }

    public override void CheckIfCompleted()
    {  
        //Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Quaternion rot = interactingObject.transform.rotation;
           
            //delta angle gets the shortest angle between two angles (180, -180) E.g., what could be 190, will be -170
            //this handles within an interaction of action object entering before exiting receiver object, user's interaction is greater than 180 or less than -180
            
            float deltaAngle = Mathf.DeltaAngle(rot.eulerAngles.z, interactingObjAngleInitial);
            currentProgress = deltaAngleBuffer + deltaAngle;
            if (deltaAngle > 175)
            {
                deltaAngleBuffer += 175;
                interactingObjAngleInitial = rot.eulerAngles.z;
            }
            else if (deltaAngle < -175)
            {
                deltaAngleBuffer -= 175;
                interactingObjAngleInitial = rot.eulerAngles.z;
            }

            if ((requirement < 0 && (pastProgress + currentProgress < stopMoveBuffer)) || (requirement >= 0 && pastProgress + currentProgress > stopMoveBuffer))
            {
                
                totalProgress = pastProgress + currentProgress;
                //Debug.Log("current progress: " + currentProgress);
                Debug.Log("total progress: " + totalProgress);
                ShowFeedback();
            }
        }
        else
        {
            interactingObject = null;
            audioData.Pause();
        }
        
        if ((requirement < 0 && (totalProgress <= requirement)) || (requirement >= 0 && (totalProgress >= requirement)))
        {
            if (checkTime = false)
            {
                checkTime = true;
                timeStart = Time.time;
                Debug.Log(timeStart);
            }
        }
        else
        {
            checkTime = false;
            timeStart = 0;
        }
    }
}
