using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ObjectGrabbedState is the state when the object is being grabbed by the player's hands or controllers. Transitions to GrabHoverState when the trigger button is pressed and a proper collision with the appropriate object takes place.
/// </summary>
public class ObjectGrabbedState : ObjectBaseState
{
    ObjectStateManager iOsm;

    public override void EnterState(ObjectStateManager osm)
    {
        this.iOsm = osm;
        // this.iOsm.GetComponent<MeshRenderer>().material = osm.onGrabMat;
        this.iOsm.isGrabbed = true;
        Debug.Log("Holding a " + this.iOsm.gameObject.name);
    }

    // Might need to restructure to check whether the trigger is on, then check for emit ray to avoid consistent checks in UpdateState()
    public override void UpdateState(ObjectStateManager osm)
    {
        if(this.iOsm.EmitRay() && this.iOsm.isTriggerOn)
        {
            this.iOsm.SwitchState(this.iOsm.objectGrabHoverState);
        }
    }
}
