using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitActionComponent : BaseActionComponent
{
    [SerializeField] GameObject interactingObject;
    public float distance;
    /* public float requiredTotalForce;
    public float accumulatedForce;
    public float minimumForce; */
    public int requiredHits;
    int accumulatedHits;

    GameObject parentObject;
    public GameObject destroyedVersion;

    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null; 
        parentObject = gameObject.transform.parent.gameObject;   
        // accumulatedForce = 0;    
        accumulatedHits = 0;
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
            accumulatedHits += 1;
        }
    }

    //when is this used?
    public bool CheckAction()
    {
        bool distanceCheck = false;

        distanceCheck = Vector3.Distance(interactingObject.transform.position, gameObject.transform.position) <= distance;

        return distanceCheck;
    }

    public void DestroyObject() {
        Instantiate(destroyedVersion, parentObject.transform.position, parentObject.transform.rotation);
        Destroy(parentObject);
    }

    public override void CheckIfCompleted()
    {   
        Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Vector3 pos = interactingObject.transform.position;
            Quaternion rot = interactingObject.transform.rotation;
            Debug.Log("accumulated Hits: " + accumulatedHits);

        }
        else
        {
            interactingObject = null;
        }

        if (accumulatedHits >= requiredHits)
        {
            isCompleted = true;
            Debug.Log("Hitting action completed on " + gameObject.transform.parent.name);
            DestroyObject();
        }
    }

}