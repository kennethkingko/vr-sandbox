using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] private Material unlockedMaterial; // Reference to the unlocked material

    private void Start()
    {
        // Subscribe to the LockManager's event
        LockManager lockManager = FindObjectOfType<LockManager>();
        if (lockManager != null)
        {
            lockManager.OnAllUnlocked += ChangeMaterial;
        }
    }

    // Method to change the material of the GameObject this script is attached to
    private void ChangeMaterial()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null && unlockedMaterial != null)
        {
            objectRenderer.material = unlockedMaterial;
        }
    }

    // Public method to set the unlocked material from the Inspector
    public void SetUnlockedMaterial(Material newMaterial)
    {
        unlockedMaterial = newMaterial;
    }
}
