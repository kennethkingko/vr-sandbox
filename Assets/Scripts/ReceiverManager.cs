using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverManager : MonoBehaviour
{
    public List<BaseActionComponent> receiverObjects;
    int numReceiverObjects;
    bool isCompleted;
        
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("numReceiverObjects = " + receiverObjects.Count);
        isCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (receiverObjects.Count == 0 && !isCompleted)
        {
            ReceiverAction();
            isCompleted = true;
            this.enabled = false;
        }
        else
        {
            foreach (BaseActionComponent ro in receiverObjects) 
            {
                if (ro.isCompleted == true) {
                    receiverObjects.Remove(ro);
                    Debug.Log("numReceiverObjects = " + receiverObjects.Count);
                }
            }
        }
        
    }

    public void ReceiverAction()
    {
        Rigidbody gameObjectsRigidBody = gameObject.AddComponent<Rigidbody>();
        gameObjectsRigidBody.useGravity = true;
    }
}
