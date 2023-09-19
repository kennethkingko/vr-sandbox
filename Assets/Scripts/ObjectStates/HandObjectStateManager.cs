using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjectStateManager : ObjectStateManager
{
    [SerializeField] public GameObject hand;

    protected virtual void Awake()
    {
        meshRenderer = hand.GetComponent<MeshRenderer>();
        //Debug.Log(gameObject.name + " - " + meshRenderer);
    }

    protected override void Start()
    {
        colliderObjects = new List<GameObject>();
        _layerMask = LayerMask.GetMask("Colliders");
        
        this.currentState = objectGrabbedState;
        this.currentState.EnterState(this);
        // this.GetComponent<MeshRenderer>().material = defaultMat;
    }

    public override void HandleGrabState()
    {
        if (this.currentState is ObjectIdleState)
        {
            this.SwitchState(objectGrabbedState);
        }
    }
}
