using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The HoverActionComponent is an action component that mimics hover action when the interacting object is near to the action component. Uses distance and time for computation of completion. This is a sample action component only.
/// </summary>
public class HoverActionComponent : BaseActionComponent
{
    GameObject interactingObject;
    float currentTime;
    public float time;
    public float distance;

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
            currentTime = 0;
        }
    }

    public override void CheckIfCompleted()
    {   
        Debug.Log("Check if completed");
        if (Vector3.Distance(interactingObject.transform.position, gameObject.transform.position) <= distance && interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {  
            currentTime += Time.deltaTime;
            Debug.Log("Checking time: " + currentTime);
        }
        else
        {
            currentTime = 0;
        }

        if (currentTime >= time)
        {
            isCompleted = true;
            Debug.Log("Hover action completed");
        }
    }
}