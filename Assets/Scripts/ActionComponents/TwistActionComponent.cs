using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistActionComponent : BaseActionComponent
{

    [SerializeField] GameObject interactingObject;
    public float distance;
    public float requiredAngle;
    public float minimumTurnAngle;
    [SerializeField] float angle;
    [SerializeField] Transform entryTransform;
    float zAngle;
    float parentZ;
    int turnCount;
    
    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
    }

    public override void Update()
    {
        if (!this.isCompleted && interactingObject != null) CheckIfCompleted();
    }

    public override void OnEntry(GameObject go)
    {
        if (go != null && !this.isCompleted)
        {
            interactingObject = go;
            Vector3 pos = go.transform.position;
            Quaternion rot = go.transform.rotation;
            // zAngle = rot.z;
            zAngle = rot.eulerAngles.z + angle;
            // Debug.Log("Entry transform: " + pos + " " + rot);            
            GameObject parentObject = gameObject.transform.parent.gameObject;
            parentZ = parentObject.transform.eulerAngles.z; 
            
        }
    }

    //when is this used?
    public float CheckAction()
    {
        float actedAngle = 0.0f;
        Quaternion rot = interactingObject.transform.rotation;

        // Change the computation here
        actedAngle = Mathf.Clamp(zAngle - rot.eulerAngles.z, 0, 360);
        // actedAngle = Mathf.DeltaAngle(rot.eulerAngles.z, zAngle);

        return actedAngle;
    }

    public override void CheckIfCompleted()
    {   
        float actedAngle = 0.0f;
        Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Quaternion rot = interactingObject.transform.rotation;

            //angle += rot.z - zAngle;

            // works more accurately but angle has to be less than 180/greater than -180
            // problem is if it goes in the wrong direction, once it reaches 180, will immediately switch to the other direction
            // clockwise is positive
            actedAngle = CheckAction();
            Debug.Log("Current angle: (" + angle + " - " + rot.eulerAngles.z + " - "+ zAngle +")");
            GameObject parentObject = gameObject.transform.parent.gameObject;
            if (Mathf.Abs((parentZ + actedAngle) - parentObject.transform.eulerAngles.z)> 1) {
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentObject.transform.eulerAngles.y,
                parentZ+actedAngle);
                // Debug.Log("parentObject: " + parentObject.transform.eulerAngles.y);
            }
            angle = actedAngle;
        }
        if (angle >= requiredAngle)
        {
            this.isCompleted = true;
            Debug.Log("Twisting action completed on " + gameObject.transform.parent.name);
        }
    }

}