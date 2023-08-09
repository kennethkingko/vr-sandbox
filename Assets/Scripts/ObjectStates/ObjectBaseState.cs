using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ObjectBaseState is the abstract base state to which other states are derived. Extend when needed. Ideally, only object state maangers need to be extended. However, if certain functionalities are needed that require extending the other states, as long as the state transitions are proper, it could be implemented as such.
/// </summary>
public abstract class ObjectBaseState
{
    public abstract void EnterState(ObjectStateManager osm);
    public abstract void UpdateState(ObjectStateManager osm);
}
