using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class InventoryObjectStateManager : ObjectStateManager
{
    void Awake()
    {
        gameObject.AddComponent<XRItemInteractionScriptV2>();
        interactable = gameObject.GetComponent<XRItemInteractionScriptV2>();
    }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     colliderObjects = new List<GameObject>();
    //     _layerMask = LayerMask.GetMask("Colliders");
    //     this.currentState = objectIdleState;
    //     this.currentState.EnterState(this);
    //     this.GetComponent<MeshRenderer>().material = defaultMat;
    // }

    // // private void Awake()
    // // {
    // //     interactor = GetComponent<XRGrabInteractable>();
    // // }

    // // Update is called once per frame
    // void Update()
    // {
    //     this.currentState.UpdateState(this);

    //     HandleGrabState();
    //     HandleTrigger();
    // }

    // public void SwitchState(ObjectBaseState state)
    // {
    //     this.currentState = state;
    //     this.currentState.EnterState(this);
    // }

    // public void HandleGrabState()
    // {
    //     if (interactable.isSelected && this.currentState is ObjectIdleState)
    //     {
    //         EnterGrabbedState();
    //     }
    //     if (!interactable.isSelected)
    //     {
    //         ExitGrabbedState();
    //     }
    // } 

    // public void EnterGrabbedState()
    // {
    //     this.SwitchState(objectGrabbedState);
    // }

    // public void ExitGrabbedState()
    // {
    //     this.SwitchState(objectIdleState);
    // }

    // public void HandleTrigger()
    // {
    //     var devices = new List<InputDevice>();
    //     InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand, devices);
        
    //     foreach (var device in devices)
    //     {
    //         bool featureValue;
    //         if (device.TryGetFeatureValue(CommonUsages.triggerButton, out featureValue) && featureValue)
    //         {
    //             Debug.Log("Trigger on!");
    //             this.isTriggerOn = true;
    //         }
    //         else
    //         {
    //             this.isTriggerOn = false;
    //         }
    //     }   
        
    // }

    // public void TriggerPressed()
    // {
    //     this.isTriggerOn = true;
    // }

    // public void TriggerReleased()
    // {
    //     this.isTriggerOn = false;
    // }

    // public bool IsHitObjectWithinAngle(RaycastHit hit, Vector3 start, Vector3 end, float theta)
    // {
    //     float deg = Vector3.Angle(hit.transform.position - start, end - start);

    //     if (deg <= theta)
    //     {
    //         return true;
    //     }
    //     return false;
    // }

    // public bool IsObjectWithinDistance(RaycastHit hit, float distance)
    // {
    //     if (hit.distance <= distance)
    //     {
    //         return true;
    //     }
    //     return false;
    // }

    // public bool EmitRay()
    // {
    //     RaycastHit hit;
    //     bool isHitting;
    //     Vector3 start, end;

    //     start = this.raycastOrigin.transform.position;
    //     end = this.raycastOrigin.transform.position + (this.raycastOrigin.transform.rotation * (this.raycastDirection * range));

    //     isHitting = Physics.Linecast(start, end, out hit, _layerMask);
    //     Debug.DrawLine(start, end, Color.green);

    //     if (isHitting && hit.transform.name != this.transform.name && IsObjectWithinDistance(hit, range) && IsHitObjectWithinAngle(hit, start, end, angle) && hit.transform.tag == "Colliders")
    //     // if (isHitting && hit.transform.name != this.transform.name)
    //     {
            
    //         float deg = Vector3.Angle(hit.transform.position - start, end - start);
    //         Debug.Log(this.transform.name + " hits: " + hit.transform.name + "(" + hit.distance + ", " + deg + ") :: " + this.raycastOrigin.transform.position + (this.raycastDirection * range));
    //         currentInteractingObject = hit.transform.gameObject;
    //         return true;
    //     }
    //     currentInteractingObject = null;
    //     return false;
    // }
}
