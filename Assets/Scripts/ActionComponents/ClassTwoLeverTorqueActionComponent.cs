using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassTwoLeverTorqueActionComponent : BaseActionComponent
{
    GameObject interactingObject;
    public float distance;
    public float requiredAngle;
    float angle;
    Transform entryTransform;

    public void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
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
            entryTransform = interactingObject.transform;
            Vector3 pos = interactingObject.transform.position;
            Quaternion rot = interactingObject.transform.rotation;
            Debug.Log("Entry transform: " + pos + " " + rot);
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
            Debug.Log("Checking orientation: " + pos + " " + rot);
        }

        if (angle >= requiredAngle)
        {
            isCompleted = true;
            Debug.Log("Turning action completed.");
        }
        // if (currentTime >= time)
        // {
        //     isCompleted = true;
        //     Debug.Log("Hover action completed");
        // }
    }
}