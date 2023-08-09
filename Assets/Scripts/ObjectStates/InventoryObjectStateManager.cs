using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// The InventoryObjectStateManager is a state manager that extends from the base ObjectStateManager for the implementation of the XRItemInteractionScriptV2 for inventory systems. This is an example of a subclass that requires a different XRGrabInteractable script.
/// </summary>
public class InventoryObjectStateManager : ObjectStateManager
{
    void Awake()
    {
        gameObject.AddComponent<XRItemInteractionScriptV2>();
        interactable = gameObject.GetComponent<XRItemInteractionScriptV2>();
    }

}
