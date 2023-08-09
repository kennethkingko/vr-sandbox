using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRItemInteractionScriptV2 : XRGrabInteractable
{
    public bool isGrabbing;

    void Start (){
        isGrabbing = false;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {

        Debug.Log("XRCustomScript V3: Item is SELECTED | " + gameObject.name);
        

        isGrabbing = true;

        if (gameObject.GetComponent<ItemScriptV3>() == null)
        {
            return;
        }
        if (gameObject.GetComponent<ItemScriptV3>().inSlot)
        {
            gameObject.GetComponent<ItemScriptV3>().currentSlot.itemInSlot = null;
            gameObject.transform.parent = null;
            gameObject.GetComponent<ItemScriptV3>().inSlot = false;
            gameObject.GetComponent<ItemScriptV3>().currentSlot.resetColor();
            gameObject.GetComponent<ItemScriptV3>().currentSlot = null;
            
        }

        base.OnSelectEntering(args);

        

    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {

        Debug.Log("XRCustomScript V3: Item is DROPPED | " + gameObject.name);

        if (!gameObject.GetComponent<ItemScriptV3>().inSlot){
            Debug.Log("Object set to false kinematic");
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        
        isGrabbing = false;

        base.OnSelectExiting(args);

        
    }







}