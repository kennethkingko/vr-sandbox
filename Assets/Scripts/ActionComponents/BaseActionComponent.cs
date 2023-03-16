using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActionComponent : MonoBehaviour
{
    public Collider actionCollider;
    public bool isCompleted;

    public abstract void Update();
    public abstract void OnEntry(GameObject go);
    public abstract void CheckIfCompleted();
}