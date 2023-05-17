using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabHoverState : ObjectBaseState
{
    ObjectStateManager iOsm;
    BaseActionComponent bsc;
    public override void EnterState(ObjectStateManager osm)
    {
        this.iOsm = osm;
        this.iOsm.GetComponent<MeshRenderer>().material = osm.onRaycastMat;
        this.iOsm.isGrabbed = true;
        bsc = this.iOsm.currentInteractingObject.GetComponent<BaseActionComponent>();
        bsc.OnEntry(this.iOsm.gameObject);
        Debug.Log("Interaction start...");
    }

    public override void UpdateState(ObjectStateManager osm)
    {
        Debug.Log("Currently interacting!");
        if (!bsc.isCompleted)
        {
            bsc.CheckIfCompleted();
        }
        if (!this.iOsm.EmitRay() || !this.iOsm.isTriggerOn)
        {
            this.iOsm.SwitchState(this.iOsm.objectGrabbedState);
        }

    }
}
