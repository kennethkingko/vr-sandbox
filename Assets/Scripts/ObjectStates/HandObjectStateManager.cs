using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjectStateManager : ObjectStateManager
{
    protected override void Start()
    {
        colliderObjects = new List<GameObject>();
        _layerMask = LayerMask.GetMask("Colliders");
        this.currentState = objectGrabbedState;
        this.currentState.EnterState(this);
        // this.GetComponent<MeshRenderer>().material = defaultMat;
    }

    void Update()
    {
        Debug.Log(gameObject.name + " - " + this.currentState);
        this.currentState.UpdateState(this);

        HandleGrabState();
        HandleTrigger();
    }

    public override void HandleGrabState()
    {
        if (this.currentState is ObjectIdleState)
        {
            this.SwitchState(objectGrabbedState);
        }
    }
}
