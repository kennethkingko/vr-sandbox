using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ClassTwoLeverTorqueActionComponent is an action component that mimics wrench-like action where torque is applied in a class-two level manner. Computation is made by checking the rotation along the y-component of the interacting object.
/// </summary>
public class ClassTwoLeverTorqueActionComponent : BaseActionComponent
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
    Vector3 interactingObjectPosInitial;
   
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
            interactingObjectPosInitial = go.transform.position;
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
            Vector3 pos = interactingObject.transform.position;

            Debug.Log("Angle: " + Vector3.SignedAngle(interactingObjectPosInitial - gameObject.transform.position, pos - gameObject.transform.position, gameObject.transform.forward));
           
            //delta angle gets the shortest angle between two angles (180, -180) E.g., what could be 190, will be -170
            //this handles within an interaction of action object entering before exiting receiver object, user's interaction is greater than 180 or less than -180
            
            float deltaAngle = Vector3.SignedAngle(interactingObjectPosInitial - gameObject.transform.position, pos - gameObject.transform.position, gameObject.transform.forward);
            currentProgress = deltaAngleBuffer + deltaAngle;
            if (deltaAngle > 120)
            {
                deltaAngleBuffer += 120;
                interactingObjectPosInitial = pos;
            }
            else if (deltaAngle < -120)
            {
                deltaAngleBuffer -= 120;
                interactingObjectPosInitial = pos;
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

        if ((requirement < 0 && totalProgress <= requirement) || (requirement >= 0 && totalProgress >= requirement))
        {
            isCompleted = true;
            Debug.Log("Turning action completed on " + gameObject.transform.parent.name);
            ShowOutcome();
        }
    }
}