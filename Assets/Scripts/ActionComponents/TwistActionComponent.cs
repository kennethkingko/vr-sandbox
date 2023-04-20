using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistActionComponent : BaseActionComponent
{

    [SerializeField] GameObject interactingObject;
    public float distance;
    public float requiredAngle;
    [SerializeField] float angle;
    [SerializeField] Transform entryTransform;
    float zAngle;
    float parentZ;
    
    void Start()
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
            // zAngle = rot.z;
            zAngle = rot.eulerAngles.z;
            Debug.Log("Entry transform: " + pos + " " + rot);            
            GameObject parentObject = gameObject.transform.parent.gameObject;
            parentZ = gameObject.transform.parent.gameObject.transform.eulerAngles.z; 
            
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
        Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Vector3 pos = interactingObject.transform.position;
            Quaternion rot = interactingObject.transform.rotation;

            //angle += rot.z - zAngle;

            // works more accurately but angle has to be less than 180/greater than -180
            // problem is if it goes in the wrong direction, once it reaches 180, will immediately switch to the other direction
            // clockwise is positive
            angle = Mathf.DeltaAngle(rot.eulerAngles.z, zAngle);
            Debug.Log("Current angle: (" + angle + " - " + rot.eulerAngles.z + " - "+ zAngle +")");
            GameObject parentObject = gameObject.transform.parent.gameObject;
            if (Mathf.Abs((parentZ+angle) - parentObject.transform.eulerAngles.z)> 1) {
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentObject.transform.eulerAngles.y,
                parentZ+angle);
                Debug.Log("parentObject: " + parentObject.transform.eulerAngles.y);
            }
        }
        else
        {
            interactingObject = null;
        }

        if (angle >= requiredAngle)
        {
            isCompleted = true;
            Debug.Log("Twisting action completed on " + gameObject.transform.parent.name);
        }
    }

}