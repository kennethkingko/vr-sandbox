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

    public void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
    }
    public override void Update()
    {
        if (!isCompleted)
        {
            if (interactingObject != null) CheckIfCompleted();
        }

        // if (Vector3.Distance(interactingObject.transform.position, gameObject.transform.position) > distance)
        // {
        //     interactingObject = null;
        // }
    }

    public override void OnEntry(GameObject go)
    {
        if (go != null)
        {
            interactingObject = go;
            // entryTransform = go.transform;
            Vector3 pos = go.transform.position;
            Quaternion rot = go.transform.rotation;
            yAngle = rot.y;
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

            angle += rot.y - yAngle;
            Debug.Log("Current angle: (" + rot.y + " - "+ yAngle +")");
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
        // if (currentTime >= time)
        // {
        //     isCompleted = true;
        //     Debug.Log("Hover action completed");
        // }
    }
}