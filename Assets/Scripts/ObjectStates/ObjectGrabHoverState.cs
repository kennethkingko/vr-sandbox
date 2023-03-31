using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabHoverState : ObjectBaseState
{
    ObjectStateManager iOsm;
    public override void EnterState(ObjectStateManager osm)
    {
        this.iOsm = osm;
        this.iOsm.GetComponent<MeshRenderer>().material = osm.onRaycastMat;
        this.iOsm.isGrabbed = true;
        this.iOsm.currentInteractingObject.GetComponent<BaseActionComponent>().OnEntry(this.iOsm.gameObject);
    }

    public override void UpdateState(ObjectStateManager osm)
    {
        this.iOsm.currentInteractingObject.GetComponent<BaseActionComponent>().CheckIfCompleted();
        if (!this.iOsm.EmitRay())
        {
            this.iOsm.SwitchState(this.iOsm.objectGrabbedState);
        }

    }
}
