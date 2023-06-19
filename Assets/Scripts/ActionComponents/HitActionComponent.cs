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
    
    Vector3 posStart;
    float timeStart;
    float objectHeight;
    public float range;
    bool hitAlready;
    public float totalPower;
    float currentPower;

    GameObject parentObject;
    public GameObject destroyedVersion;

    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null; 
        parentObject = gameObject.transform.parent.gameObject;   
        // accumulatedForce = 0;    
        accumulatedHits = 0;
        currentPower = 0;
        objectHeight = parentObject.GetComponent<Renderer>().bounds.size.z;
        //destroyedVersion.SetActive(false);
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

            /* // simple version
            accumulatedHits += 1; */

            // complex version
            posStart = interactingObject.transform.position;
            timeStart = Time.time;
            hitAlready = false;
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
        //destroyedVersion.SetActive(true);
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
            // Debug.Log("accumulated Hits: " + accumulatedHits);

            // complex version
            if (Vector3.Distance(pos, gameObject.transform.position) < (objectHeight/2)+10 && !hitAlready)
            {
                float timeEnd = Time.time;
                float power = Vector3.Distance(posStart, gameObject.transform.position)/ (timeEnd-timeStart);
                Debug.Log(power + " hit! ");
                currentPower += power;
                hitAlready = true;
            }
        }
        else
        {
            interactingObject = null;
        }
        
        /* // simple version
        if (accumulatedHits >= requiredHits)
        {
            isCompleted = true;
            Debug.Log("Hitting action completed on " + gameObject.transform.parent.name);
            DestroyObject();
        } */

        // complex version
        if (currentPower >= totalPower)
        {
            isCompleted = true;
            Debug.Log("Hitting action completed on " + gameObject.transform.parent.name);
            DestroyObject();
        }
    }

}