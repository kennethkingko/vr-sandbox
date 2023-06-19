using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TwistActionComponent is an action component that mimics twisting action like opening a doorknob or using a screwdriver. Computation is made by checking the rotation along the z-component of the interacting object.
/// </summary>
public class TwistActionComponent : BaseActionComponent
{
    // Values needed for the acttion to be completed
    public bool simpler;
    public float requiredAngle;
    
    public float distance;
    
    float angle;
    GameObject interactingObject;

    float zAngle;
    
    float parentZ;
    float parentZPos;
    float parentZPosInitial;
    float objectLength;
    GameObject parentObject;
    
    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
        parentObject = gameObject.transform.parent.gameObject;
        objectLength = parentObject.GetComponent<Renderer>().bounds.size.z;
        parentZPosInitial = parentObject.transform.position.z; 
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
            // zAngle = rot.z;
            zAngle = rot.eulerAngles.z;
            //Debug.Log("Entry transform: " + pos + " " + rot);        
            parentZ = parentObject.transform.eulerAngles.z; 
            parentZPos = parentObject.transform.position.z; 
        }
    }

    //when is this used?
    public bool CheckAction()
    {
        bool distanceCheck = false;

        distanceCheck = Vector3.Distance(interactingObject.transform.position, gameObject.transform.position) <= distance;

        return distanceCheck;
    }

    public override void CheckIfCompleted()
    {   
        //Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Quaternion rot = interactingObject.transform.rotation;

            //angle += rot.z - zAngle;

            // works more accurately but angle has to be less than 180/greater than -180
            // problem is if it goes in the wrong direction, once it reaches 180, will immediately switch to the other direction
            // clockwise is positive
            angle = Mathf.DeltaAngle(rot.eulerAngles.z, zAngle);
            //Debug.Log("Current angle: (" + angle + " - " + rot.eulerAngles.z + " - "+ zAngle +")");
            if (Mathf.Abs((parentZ+angle) - parentObject.transform.eulerAngles.z)> 1)
            {
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentObject.transform.eulerAngles.y,
                parentZ+angle);
                Debug.Log("parentObject: " + parentObject.transform.eulerAngles.y);
                float addedZ = 0;
                if (simpler)
                {
                    addedZ = (objectLength/requiredAngle)*angle;
                }
                else
                {
                    addedZ = (objectLength/6)/(360/angle);
                }
                parentObject.transform.position = new Vector3(
                parentObject.transform.position.x,
                parentObject.transform.position.y,
                parentZPos + addedZ);
            }
            
            Debug.Log("Limit: " + (parentZPosInitial + objectLength) + " - Current: " + parentObject.transform.position.z);
        }
        else
        {
            interactingObject = null;
        }

        if (parentObject.transform.position.z >= parentZPosInitial + objectLength)
        {
            isCompleted = true;
            Debug.Log("Twisting action completed on " + gameObject.transform.parent.name);
            Rigidbody gameObjectsRigidBody = parentObject.AddComponent<Rigidbody>();
            gameObjectsRigidBody.useGravity = true;
        }
    }

}