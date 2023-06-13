using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The BaseActionComponent is the abstract base state for all action components to be created. Action comopnent scripts are added to the colliders of the object where interaction takes place, not on the object itself. This means multiple action components, with equal number of colliders, can exist in one object. Extend when needed.
/// </summary>
public abstract class BaseActionComponent : MonoBehaviour
{
    public Collider actionCollider;
    public bool isCompleted;

    public abstract void Update();
    public abstract void OnEntry(GameObject go);
    public abstract void CheckIfCompleted();
}