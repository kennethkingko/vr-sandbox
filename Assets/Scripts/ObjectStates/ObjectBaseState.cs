using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBaseState
{
    public abstract void EnterState(ObjectStateManager osm);
    public abstract void UpdateState(ObjectStateManager osm);
}
