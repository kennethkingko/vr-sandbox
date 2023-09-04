using UnityEngine;

public class ChangeMaterialOnClick : MonoBehaviour
{
    public Material newMaterial; // The material to change to

    private void OnMouseDown()
    {
        // Check if the mouse click occurred over this object
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object is the same as this object
            if (hit.collider.gameObject == gameObject)
            {
                // Change the material
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer != null && newMaterial != null)
                {
                    meshRenderer.material = newMaterial;
                }
            }
        }
    }
}
