using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitActionComponent : BaseActionComponent
{
    public bool simpler;
    public float minDistance;
    GameObject interactingObject;

    Vector3 posStart;
    float timeStart;
    public float range;
    bool hitAlready;

    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
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
            Vector3 pos = go.transform.position;
            posStart = interactingObject.transform.position;
            hitAlready = false;
            timeStart = Time.time;
            currentProgress = 0;
        }
    }

    public override void CheckIfCompleted()
    {   
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Vector3 pos = interactingObject.transform.position;
            float actorReceiverDistance = Vector3.Distance(pos, gameObject.transform.position);
            float startEndDistance = Vector3.Distance(posStart, pos);
            //Debug.Log("Dist: " + startEndDistance);
            if (!hitAlready && actorReceiverDistance < range && actorReceiverDistance < Vector3.Distance(posStart, gameObject.transform.position) && startEndDistance > minDistance)
            {
                if(simpler)
                {
                    currentProgress = 1;
                }
                else{
                    float timeEnd = Time.time;
                    float timeTotal = timeEnd-timeStart;
                    currentProgress = startEndDistance/timeTotal;
                }
                totalProgress += currentProgress;
                hitAlready = true;
                //Debug.Log("totalProgress: " + totalProgress);
                ShowFeedback();
            }
        }
        else
        {
            interactingObject = null;
        }
        if (totalProgress >= requirement)
        {
            isCompleted = true;
            Debug.Log("Hitting action completed on " + gameObject.transform.parent.name);
            ShowOutcome();
        }
    }

}