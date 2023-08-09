using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActionComponent : BaseActionComponent
{
    [SerializeField] GameObject interactingObject;
    GameObject parentObject;
    public GameObject destroyedVersion;

    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null; 
        parentObject = gameObject.transform.parent.gameObject; 
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

     public void DestroyObject() {
        Instantiate(destroyedVersion, parentObject.transform.position, parentObject.transform.rotation);
        Destroy(parentObject);
    }

    public override void CheckIfCompleted()
    {   
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            interactingObject = null;
            isCompleted = true;
            Debug.Log("Action completed on " + gameObject.transform.parent.name);
            DestroyObject();
        }
    }
}
