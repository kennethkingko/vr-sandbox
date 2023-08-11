using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TwistActionComponent is an action component that mimics twisting action like opening a doorknob or using a screwdriver. Computation is made by checking the rotation along the z-component of the interacting object.
/// </summary>
public class TwistActionComponent : BaseActionComponent
{
    // Set in Inspector
    public float requiredAngle;
    public float angleSensitivity = 3;
    public Vector3 boundsDirection;
    public bool isClockwise;
    public bool isBackward;
    public float stopMoveBuffer = 0;

    //
    GameObject interactingObject;
    float totalDeltaAngle; 
    float deltaAngleBuffer;
    float interactingObjAngleInitial;
    float totalMoved;
   
    GameObject parentObject;
    float parentObjectLength;
    float parentObjAngleInitial;
   
    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
        parentObject = gameObject.transform.parent.gameObject;
        totalMoved = 0;


        if(boundsDirection.x > 0) {
            parentObjectLength = parentObject.GetComponent<Renderer>().bounds.size.x;
        }
        else if(boundsDirection.y > 0) {
            parentObjectLength = parentObject.GetComponent<Renderer>().bounds.size.y;
        }
        else if(boundsDirection.z > 0) {
            parentObjectLength = parentObject.GetComponent<Renderer>().bounds.size.z;
        }
        else {
            Debug.LogError("Must set bounds direction", gameObject.transform);
        }        
        //Debug.Log(gameObject.transform.parent.name + ": " + parentObjectLength);
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
            parentObjAngleInitial = parentObject.transform.eulerAngles.z;
            deltaAngleBuffer = 0;
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
            if (Mathf.Abs(deltaAngle) > angleSensitivity)
            {
                totalDeltaAngle = deltaAngleBuffer + deltaAngle;
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
            
                // object rotation
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentObject.transform.eulerAngles.y,
                parentObjAngleInitial + totalDeltaAngle);

                // object forward movement
                float move;
                if (isClockwise)
                {
                    move = (parentObjectLength/requiredAngle)*totalDeltaAngle;
                }
                else
                {
                    move = -(parentObjectLength/requiredAngle)*totalDeltaAngle;
                }
                if (totalMoved + move > -stopMoveBuffer)
                {
                    totalMoved += move;
                    if (isBackward) {
                        parentObject.transform.position -= transform.forward*move;
                    }
                    else
                    {
                        parentObject.transform.position += transform.forward*move;
                    }                
                }
            }     
        }
        else
        {
            interactingObject = null;
        }

        if (totalMoved >= parentObjectLength)
        {
            isCompleted = true;
            Debug.Log("Twisting action completed on " + gameObject.transform.parent.name);
            Rigidbody gameObjectsRigidBody = parentObject.AddComponent<Rigidbody>();
            gameObjectsRigidBody.useGravity = true;
        }
    }
}
