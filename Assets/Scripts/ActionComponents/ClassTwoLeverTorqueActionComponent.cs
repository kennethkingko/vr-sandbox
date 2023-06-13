using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ClassTwoLeverTorqueActionComponent is an action component that mimics wrench-like action where torque is applied in a class-two level manner. Computation is made by checking the rotation along the y-component of the interacting object.
/// </summary>
public class ClassTwoLeverTorqueActionComponent : BaseActionComponent
{
    // Values needed for the action to be completed
    public float distance;
    public float requiredAngle;
    
    [SerializeField] float angle;
    [SerializeField] GameObject interactingObject;

    float yAngle;
    float parentY;

    public void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
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

            yAngle = rot.eulerAngles.y + angle;
            Debug.Log("Entry transform: " + pos + " " + rot);
            GameObject parentObject = gameObject.transform.parent.gameObject;
            parentY = gameObject.transform.parent.gameObject.transform.eulerAngles.y; 
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
        Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Vector3 pos = interactingObject.transform.position;
            Quaternion rot = interactingObject.transform.rotation;

            //angle += rot.y - yAngle;

            //like in TwistingActionComponent, works more accurately
            //only comfortable until about 45 degrees tho
            //clockwise is negative
            angle = Mathf.DeltaAngle(rot.eulerAngles.y, yAngle);
            Debug.Log("Current angle: (" + angle +")");            

            GameObject parentObject = gameObject.transform.parent.gameObject;
            Debug.Log("parentObject: " + parentObject.transform.eulerAngles);

            if (Mathf.Abs((parentY-angle) - parentObject.transform.eulerAngles.y)> 3) {
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentY-angle,                
                parentObject.transform.eulerAngles.z);                
            }
        }
        else
        {
            interactingObject = null;
        }

        if (angle >= requiredAngle)
        {
            isCompleted = true;
            Debug.Log("Turning action completed on " + gameObject.transform.parent.name);
        }
    }
}