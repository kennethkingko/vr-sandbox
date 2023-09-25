using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActionComponent : BaseActionComponent
{
    GameObject interactingObject;

    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null; 
        requirement = 1;
        currentProgress = 0;
        totalProgress = 0;
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
        }
    }

    public override void CheckIfCompleted()
    {   
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            currentProgress = 1;
            totalProgress = 1;
            interactingObject = null;
            isCompleted = true;
            Debug.Log("Action completed on " + gameObject.transform.parent.name);
            ShowOutcome();
        }
    }
}