using System;
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
    public float requirement;
    public float currentProgress;
    public float totalProgress;

    public event Action Feedback;
    public event Action Outcome;

    public abstract void Update();
    public abstract void OnEntry(GameObject go);
    public abstract void CheckIfCompleted();
    
    protected virtual void ShowFeedback()
    {
        Feedback?.Invoke();
    }
    protected virtual void ShowOutcome()
    {
        Outcome?.Invoke();
    }
}