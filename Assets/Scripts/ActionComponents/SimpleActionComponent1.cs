using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActionComponent1 : BaseActionComponent
{
    [SerializeField] GameObject interactingObject;
    GameObject parentObject;
    float objectLength;

    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null; 
        parentObject = gameObject.transform.parent.gameObject; 
        objectLength = parentObject.GetComponent<Renderer>().bounds.size.z;
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

     public void MoveObject() {
        parentObject.transform.position = new Vector3(
        parentObject.transform.position.x,
        parentObject.transform.position.y,
        parentObject.transform.position.z + objectLength);
        Rigidbody gameObjectsRigidBody = parentObject.AddComponent<Rigidbody>();
        gameObjectsRigidBody.useGravity = true;
    }

    public override void CheckIfCompleted()
    {   
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            isCompleted = true;
            Debug.Log("Action completed on " + gameObject.transform.parent.name);
            MoveObject();
        }
    }
}
