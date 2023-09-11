using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectOutcome : MonoBehaviour
{
    BaseActionComponent actionComponent;
    GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        parentObject = gameObject.transform.parent.gameObject;
        actionComponent.Outcome += DropObject;
    }

    // Update is called once per frame
    void DropObject()
    {
        Rigidbody gameObjectsRigidBody = parentObject.AddComponent<Rigidbody>();
        gameObjectsRigidBody.useGravity = true;
    }
}
