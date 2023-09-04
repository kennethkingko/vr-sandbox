using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f; // Speed at which the object moves downward
    private bool isMoving = false; // Flag to track if the object should move

    private void Start()
    {
        // Subscribe to the LockManager's event
        LockManager lockManager = FindObjectOfType<LockManager>();
        if (lockManager != null)
        {
            lockManager.OnAllUnlocked += StartMoving;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move the object downward
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    private void StartMoving()
    {
        isMoving = true; // Start moving the object when all locks are unlocked
    }
}
