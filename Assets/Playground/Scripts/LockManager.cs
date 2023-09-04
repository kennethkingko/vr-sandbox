using System;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectList = new List<GameObject>();
    private bool allUnlockedTriggered = false;

    // Define an event for when all locks are unlocked
    public event Action OnAllUnlocked;

    void Update()
    {
        if (!allUnlockedTriggered)
        {
            bool allUnlocked = true;

            foreach (GameObject go in objectList)
            {
                Lock lockComponent = go.GetComponent<Lock>();

                if (lockComponent != null && lockComponent.currentState != Lock.LockState.Unlocked)
                {
                    allUnlocked = false;
                    break;
                }
            }

            if (allUnlocked)
            {
                Debug.Log("All locks are unlocked.");
                allUnlockedTriggered = true;

                // Trigger the event when all locks are unlocked
                OnAllUnlocked?.Invoke();
            }
        }
    }
}
