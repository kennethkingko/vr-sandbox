using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public enum LockState
    {
        Locked,
        Unlocked,
    }

    public LockState currentState;

    [Tooltip("The renderer whose material changes will trigger the lock.")]
    public Renderer materialRenderer;

    private Material initialMaterial;

    // Start is called before the first frame update
    void Start()
    {
        currentState = LockState.Locked;

        if (materialRenderer != null)
        {
            // Store the initial material
            initialMaterial = materialRenderer.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (materialRenderer != null)
        {
            // Check if the material has changed
            if (materialRenderer.material != initialMaterial)
            {
                Unlock();
            }
        }
    }

    public void Unlock()
    {
        if (currentState == LockState.Locked)
        {
            currentState = LockState.Unlocked;
            Debug.Log("Lock is now unlocked!");
        }
    }
}