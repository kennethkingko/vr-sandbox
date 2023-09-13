using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TwistActionComponent is an action component that mimics twisting action like opening a doorknob or using a screwdriver. Computation is made by checking the rotation along the z-component of the interacting object.
/// </summary>
public class TwistActionComponent : BaseActionComponent
{
    // Set in Inspector
    public float stopMoveBuffer = 0;
    // requirement = required angle
    
    // currentProgress = deltaAngle + deltaAngleBuffer
    // totalProgress = pastProgress + currentProgress
    GameObject interactingObject;    
    float deltaAngleBuffer;
    float interactingObjAngleInitial;
    float pastProgress;
   
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
            currentProgress = deltaAngleBuffer + deltaAngle;

            if(pastProgress - currentProgress > -stopMoveBuffer)
            {
                totalProgress = pastProgress - currentProgress;
                Debug.Log("current progress: " + currentProgress);
                Debug.Log("total progress: " + totalProgress);
                ShowFeedback();
            }               
        }
        else
        {
            interactingObject = null;
        }

        if (totalProgress >= requirement)
        {
            isCompleted = true;
            Debug.Log("Twisting action completed on " + gameObject.transform.parent.name);
            ShowOutcome();
        }
    }
}
