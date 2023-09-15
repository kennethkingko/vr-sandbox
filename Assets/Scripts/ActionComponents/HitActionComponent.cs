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
    public float requiredPower;
    float currentPower;

    GameObject parentObject;
    public GameObject destroyedVersion;
    public ParticleSystem particleSys;
    public float emissionMin;
    public float emissionMax;

    void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null; 
        parentObject = gameObject.transform.parent.gameObject;
        currentPower = 0;
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
        }
    }

    public void DestroyObject() {
        //destroyedVersion.SetActive(true);
        Instantiate(destroyedVersion, parentObject.transform.position, parentObject.transform.rotation);
        Destroy(parentObject);
    }

    public override void CheckIfCompleted()
    {   
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Vector3 pos = interactingObject.transform.position;
            float actorReceiverDistance = Vector3.Distance(pos, gameObject.transform.position);
            float startEndDistance = Vector3.Distance(posStart, pos);
            Debug.Log("Dist: " + startEndDistance);
            if (!hitAlready && actorReceiverDistance < range && actorReceiverDistance < Vector3.Distance(posStart, gameObject.transform.position) && startEndDistance > minDistance)
            {
                if(simpler)
                {
                    currentPower += 1;
                }
                else{
                    float timeEnd = Time.time;
                    float timeTotal = timeEnd-timeStart;
                    
                    currentPower += startEndDistance/timeTotal;
                }
                hitAlready = true;
                particleSys.Play();
                Debug.Log("current power: " + currentPower);

                /* if (simpler)
                {
                    //em.rateOverTime = emissionMin + ((emissionMax-emissionMin)*(accumulatedHits/requiredHits));
                } 
                else
                {                    
                    //em.rateOverTime = emissionMin + ((emissionMax-emissionMin)*(power/10));
                    
                }          */
            }
        }
        else
        {
            interactingObject = null;
        }
        if (currentPower >= requiredPower)
        {
            isCompleted = true;
            Debug.Log("Hitting action completed on " + gameObject.transform.parent.name);
            particleSys.Play();
            DestroyObject();
        }
    }

}