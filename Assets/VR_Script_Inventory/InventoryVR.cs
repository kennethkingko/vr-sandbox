// Script name: InventoryVR
// Script purpose: attaching a gameobject to a certain anchor and having the ability to enable and disable it.
// This script is a property of Realary, Inc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class InventoryVR : MonoBehaviour
{   

    private enum ControllerSide{
        Left_Controller,
        Right_Controller,
    }
    [SerializeField]
    private ControllerSide mainController;

    private InputDeviceCharacteristics mainControllerCharacteristics;

    [SerializeField]
    public GameObject Inventory;
    [SerializeField]
    public GameObject Anchor;

    [SerializeField]
    public GameObject Anchor_hand;
    [SerializeField]
    public GameObject buttonWorksTest;
    [SerializeField]
    public GameObject gripWorksTest;

    [SerializeField]
    public GameObject inventoryColliderCheck;

    [Header("=========")]

    public GameObject itemColliding;
    public GameObject slotColliding;

    public bool debugHandIsFalse;

    [Header("=========")]

    public List<InputDevice> mainDevice = new List<InputDevice>();
    public List<InputDevice> subDevice = new List<InputDevice>();
    
    bool UIActive;

    [SerializeField]
    float buttonDelay = 0.3f;
    
    // public InputDevice mainDevice;

    [Header("=========")]



    float lastPressTime = 0f;

    private bool m_debugMode = true;

    void Start()
    {
        Inventory.SetActive(false);
        UIActive = false;

        // if (DebugLogger.current == null) {m_debugMode = false;}
        if (mainController == ControllerSide.Left_Controller){
            mainControllerCharacteristics = InputDeviceCharacteristics.Left;
        } else {
            mainControllerCharacteristics = InputDeviceCharacteristics.Right;
        }

        Debug.Log("main device init");
    }

    void Update()
    {
        // if(Anchor.transform.parent.gameObject.activeSelf){
        //     Anchor_hand.transform.position = Anchor.transform.position;
        // }
        
        
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left,mainDevice);
        
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right,subDevice);

        if (mainDevice.Count==1){
            // Device found
            openCloseInventory(mainDevice[0]);
        } else {
            // Debug.Log("Controller not found:");
        }


        if (subDevice.Count>=1){
            // Device found
            releaseGrip(subDevice[0]);
            // Debug.Log("R CTRL F");
        } else {
            // Debug.Log("Controller not found:");
        }
        
        if (lastPressTime > 0f){
            lastPressTime -= Time.deltaTime;
        }
        
        
        if (Inventory.activeSelf)
        {
            if (Anchor.transform.parent.gameObject.activeSelf){
                debugHandIsFalse = true;
                Inventory.transform.position = Vector3.Lerp(Inventory.transform.position,Anchor.transform.position,1);
                Inventory.transform.eulerAngles = new Vector3(Anchor.transform.eulerAngles.x + 15, Anchor.transform.eulerAngles.y, 0);
            } 
            
            if (Anchor_hand.transform.parent.gameObject.activeSelf && !Anchor.transform.parent.gameObject.activeSelf){
                debugHandIsFalse = false;
                Inventory.transform.position = Vector3.Lerp(Inventory.transform.position,Anchor_hand.transform.position,1);
                Inventory.transform.eulerAngles = new Vector3(Anchor_hand.transform.eulerAngles.x + 15, Anchor_hand.transform.eulerAngles.y, 0);
            }
            
        }


        // Debug.Log(itemColliding+" | "+slotColliding);


    }



    private void openCloseInventory(InputDevice x){

        bool primaryButtonPressed = false;
        x.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out primaryButtonPressed);
        if (primaryButtonPressed && lastPressTime <= 0){
                // Debug.Log("Primary button pressed!");
                buttonWorksTest.SetActive(!buttonWorksTest.activeSelf);
                Inventory.SetActive(!Inventory.activeSelf);
                lastPressTime = buttonDelay;
        } 
    }

    private void releaseGrip(InputDevice x){
        bool gripButtonPressed;
        x.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out gripButtonPressed);
        if (gripButtonPressed){
            // Debug.Log("Trigger test");
            gripWorksTest.SetActive(!gripWorksTest);
        }

    }

    public void addRemoveItem(){


    }



}
