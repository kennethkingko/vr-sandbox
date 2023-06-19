using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ClassTwoLeverTorqueActionComponent is an action component that mimics wrench-like action where torque is applied in a class-two level manner. Computation is made by checking the rotation along the y-component of the interacting object.
/// </summary>
public class ClassTwoLeverTorqueActionComponent : BaseActionComponent
{
    // Values needed for the action to be completed
    public bool simpler;
    public int numTimesReq;
    public int numTimesCurrent;
    public float minAngle;
    public bool checkedAlready;
    public float requiredAngle;

    public float distance;
    
    [SerializeField] float angle;
    [SerializeField] GameObject interactingObject;

    float yAngle;
    float parentY;

    float parentZPos;
    float parentZPosInitial;
    float objectLength;
    GameObject parentObject;

    public void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
        
        parentObject = gameObject.transform.parent.gameObject;
        objectLength = parentObject.GetComponent<Renderer>().bounds.size.z;
        parentZPosInitial = parentObject.transform.position.z; 
        numTimesCurrent = 0;
    }
    
    // Might need to be refactored for optimization
    public override void Update()
    {
        if (!isCompleted && interactingObject != null) CheckIfCompleted();
    }

    // Function check to store the initial values of the interacting object
    public override void OnEntry(GameObject go)
    {
        if (go != null)
        {
            interactingObject = go;
            Vector3 pos = go.transform.position;
            Quaternion rot = go.transform.rotation;
            //yAngle = rot.y;

            //yAngle = rot.eulerAngles.y + angle;
            yAngle = rot.eulerAngles.y;
            //Debug.Log("Entry transform: " + pos + " " + rot);
            parentY = parentObject.transform.eulerAngles.y; 
            parentZPos = parentObject.transform.position.z;
            checkedAlready = false; 
            
        }
    }

    public bool CheckAction()
    {
        bool distanceCheck = false;

        distanceCheck = Vector3.Distance(interactingObject.transform.position, gameObject.transform.position) <= distance;

        return distanceCheck;
    }

    public override void CheckIfCompleted()
    {   
        // Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Quaternion rot = interactingObject.transform.rotation;

            //angle += rot.y - yAngle;

            //like in TwistingActionComponent, works more accurately
            //only comfortable until about 45 degrees tho
            //clockwise is negative
            angle = Mathf.DeltaAngle(rot.eulerAngles.y, yAngle);
            //Debug.Log("Current angle: (" + angle +")");            

            /* if (Mathf.Abs((parentY-angle) - parentObject.transform.eulerAngles.y)> 3) {
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentY-angle,                
                parentObject.transform.eulerAngles.z);                
            } */
            if (simpler)
            {
                Debug.Log("angle: " + angle);
                if (angle <= minAngle && !checkedAlready)
                {
                    numTimesCurrent += 1;
                    parentObject.transform.position = new Vector3(
                    parentObject.transform.position.x,
                    parentObject.transform.position.y,
                    parentZPos + (objectLength/numTimesReq));
                    checkedAlready = true;
                }
            }
            else
            {
                if (angle < 0) {
                    if (Mathf.Abs((parentY-angle) - parentObject.transform.eulerAngles.y)> 3) {
                        parentObject.transform.eulerAngles = new Vector3(
                        parentObject.transform.eulerAngles.x,
                        parentObject.transform.eulerAngles.y,
                        parentY-angle);           

                        parentObject.transform.position = new Vector3(
                        parentObject.transform.position.x,
                        parentObject.transform.position.y,
                        parentZPos - (objectLength/6)/(360/angle));     
                    }
                    Debug.Log("Angle: " + angle + " - Current: " + parentObject.transform.position.z);
                }
            }            
        }
        else
        {
            interactingObject = null;
        }

        if (simpler)
        {
            if (numTimesCurrent >= numTimesReq)
            {
                isCompleted = true;
                Debug.Log("Turning action completed on " + gameObject.transform.parent.name);
                Rigidbody gameObjectsRigidBody = parentObject.AddComponent<Rigidbody>();
                gameObjectsRigidBody.useGravity = true;
            }
        }
        else
        {
            if (parentObject.transform.position.z >= parentZPosInitial + objectLength - 0.1)
            {
                isCompleted = true;
                Debug.Log("Turning action completed on " + gameObject.transform.parent.name);
                Rigidbody gameObjectsRigidBody = parentObject.AddComponent<Rigidbody>();
                gameObjectsRigidBody.useGravity = true;
            }
        }
    }
}