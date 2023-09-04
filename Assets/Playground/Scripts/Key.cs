using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Lock lockObject; // Reference to the Lock script on the lock object

    private Renderer subjectRenderer;
    private Color initialColor;

    // Start is called before the first frame update
    void Start()
    {
        subjectRenderer = GetComponent<Renderer>();

        if (subjectRenderer != null)
        {
            // Store the initial color
            initialColor = subjectRenderer.material.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (subjectRenderer != null)
        {
            // Check if the color has changed
            if (subjectRenderer.material.color != initialColor)
            {
                // Change the state of the lock object when the color changes
                if (lockObject != null)
                {
                    lockObject.Unlock();
                }
            }
        }
    }
}
