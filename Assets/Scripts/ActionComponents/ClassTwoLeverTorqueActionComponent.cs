using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassTwoLeverTorqueActionComponent : BaseActionComponent
{
    [SerializeField] GameObject interactingObject;
    public float distance;
    public float requiredAngle;
    [SerializeField] float angle;
    [SerializeField] Transform entryTransform;
    float yAngle;
    float parentZ;

    public void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
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
            //yAngle = rot.y;

            yAngle = rot.eulerAngles.y;
            Debug.Log("Entry transform: " + pos + " " + rot);
            GameObject parentObject = gameObject.transform.parent.gameObject;
            parentZ = gameObject.transform.parent.gameObject.transform.eulerAngles.y; 
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

            if (Mathf.Abs((parentZ-angle) - parentObject.transform.eulerAngles.y)> 3) {
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentZ-angle,                
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