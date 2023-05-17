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

    private enum ControllerSide
    {
        Left_Controller,
        Right_Controller,
    }

    [Header("========= General setup")]
    [SerializeField] private ControllerSide mainController;
    private InputDeviceCharacteristics mainControllerCharacteristics;
    [SerializeField] public GameObject Anchor;
    [SerializeField] public GameObject Anchor_hand;
    [SerializeField] public GameObject buttonWorksTest;
    [SerializeField] public GameObject gripWorksTest;
    [SerializeField] public GameObject inventoryColliderCheck;

    [Header("=========")]
    // public GameObject itemColliding;
    // public GameObject slotColliding;
    public bool debugHandIsFalse;

    [Header("========= Device debug")]
    public List<InputDevice> mainDevice = new List<InputDevice>();
    public List<InputDevice> subDevice = new List<InputDevice>();
    bool UIActive;

    [SerializeField]
    float buttonDelay = 0.3f;

    // public InputDevice mainDevice;

    [Header("======= Inventory systems")]


    // possible inventory systems so far!!!: 'slotsdefault', 'shelf', 'wristbased'

    public string currentSelectedInventorySystem;

    public string wantedInventorySystem;

    public bool changeInventoryStateCalled;
    [SerializeField] public GameObject HandInventory;
    [SerializeField] public GameObject ShelfInventory;
    [SerializeField] public GameObject WristInventory;

    [SerializeField] public GameObject HandbasedInventoryHierarchy;

    public List<GameObject> addedInventorySystems;

    float lastPressTime = 0f;

    private bool m_debugMode = true;
    public SlotScriptV3[] loltest;

    void Start()
    {
        changeInventoryStateCalled = false;
        currentSelectedInventorySystem = "slotsdefault";
        HandbasedInventoryHierarchy.SetActive(false);
        UIActive = false;

        // if (DebugLogger.current == null) {m_debugMode = false;}
        if (mainController == ControllerSide.Left_Controller)
        {
            mainControllerCharacteristics = InputDeviceCharacteristics.Left;
        }
        else
        {
            mainControllerCharacteristics = InputDeviceCharacteristics.Right;
        }

        Debug.Log("main device init");

        addedInventorySystems.Add(HandInventory);
        addedInventorySystems.Add(ShelfInventory);
        addedInventorySystems.Add(WristInventory);



    }

    void Update()
    {
        // if(Anchor.transform.parent.gameObject.activeSelf){
        //     Anchor_hand.transform.position = Anchor.transform.position;
        // }


        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, mainDevice);

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, subDevice);

        if (mainDevice.Count == 1)
        {
            // Device found
            openCloseInventory(mainDevice[0]);
        }
        else
        {
            // Debug.Log("Controller not found:");
        }


        if (subDevice.Count >= 1)
        {
            // Device found
            releaseGrip(subDevice[0]);
            // Debug.Log("R CTRL F");
        }
        else
        {
            // Debug.Log("Controller not found:");
        }

        if (lastPressTime > 0f)
        {
            lastPressTime -= Time.deltaTime;
        }



        if (currentSelectedInventorySystem == "slotsdefault" || currentSelectedInventorySystem == "wristbased")
        {
            if (HandbasedInventoryHierarchy.activeSelf)
            {
                if (Anchor.transform.parent.gameObject.activeSelf)
                {
                    debugHandIsFalse = true;
                    HandbasedInventoryHierarchy.transform.position = Vector3.Lerp(HandbasedInventoryHierarchy.transform.position, Anchor.transform.position, 1);
                    HandbasedInventoryHierarchy.transform.eulerAngles = new Vector3(Anchor.transform.eulerAngles.x + 15, Anchor.transform.eulerAngles.y, 0);
                }

                if (Anchor_hand.transform.parent.gameObject.activeSelf && !Anchor.transform.parent.gameObject.activeSelf)
                {
                    debugHandIsFalse = false;
                    HandbasedInventoryHierarchy.transform.position = Vector3.Lerp(HandbasedInventoryHierarchy.transform.position, Anchor_hand.transform.position, 1);
                    HandbasedInventoryHierarchy.transform.eulerAngles = new Vector3(Anchor_hand.transform.eulerAngles.x + 15, Anchor_hand.transform.eulerAngles.y, 0);
                }

            }
        }

        if (changeInventoryStateCalled)
        {
            changeInventorySystemState(currentSelectedInventorySystem, wantedInventorySystem);
            changeInventoryStateCalled = false;
        }

        // Debug.Log(itemColliding+" | "+slotColliding);

    }



    private void openCloseInventory(InputDevice x)
    {

        bool primaryButtonPressed = false;
        x.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out primaryButtonPressed);
        if (primaryButtonPressed && lastPressTime <= 0)
        {
            // Debug.Log("Primary button pressed!");
            buttonWorksTest.SetActive(!buttonWorksTest.activeSelf);
            HandbasedInventoryHierarchy.SetActive(!HandbasedInventoryHierarchy.activeSelf);
            lastPressTime = buttonDelay;
        }
    }

    private void releaseGrip(InputDevice x)
    {
        bool gripButtonPressed;
        x.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out gripButtonPressed);
        if (gripButtonPressed)
        {
            // Debug.Log("Trigger test");
            gripWorksTest.SetActive(!gripWorksTest);
        }

    }

    public void checkInventorySystemState(string systemName)
    {

        switch (systemName)
        {
            case "shelf":
                Debug.Log("Inventory System now SHELF");
                break;
            case "slotsdefault":
                Debug.Log("Inventory System now VIRTUAL SLOTS");
                break;
            case "wristbased":
                Debug.Log("Inventory System now WRISTBASED");
                break;
        }

    }


    [Header("========= Logic debug")]
    public GameObject currentInv;

    public List<GameObject> tempStorageInvMove;

    public void changeInventorySystemState(string currentSystem, string wantedSystem)
    {

        if (currentSystem == wantedSystem)
        {
            return;
        }

        switch (currentSystem)
        {
            case "shelf":
                currentInv = ShelfInventory;
                break;
            case "slotsdefault":
                currentInv = HandInventory;
                break;
            case "wristbased":
                currentInv = WristInventory;
                break;
        }

        for (int i = 0; i < currentInv.transform.childCount; i++)
        {
            if (currentInv.transform.GetChild(i).GetComponent<SlotScriptV3>() != null)
            {
                if (currentInv.transform.GetChild(i).GetComponent<SlotScriptV3>().itemInSlot)
                {
                    tempStorageInvMove.Add(currentInv.transform.GetChild(i).GetComponent<SlotScriptV3>().itemInSlot);
                    currentInv.transform.GetChild(i).GetComponent<SlotScriptV3>().ManualTriggerReleaseItem(currentInv.transform.GetChild(i).GetComponent<SlotScriptV3>().itemInSlot);
                }

            }
        }

        Debug.Log("tempStorage size:" + tempStorageInvMove.Count);

        switch (wantedSystem)
        {
            case "shelf":
                transferInventoryMethod(ShelfInventory);

                ShelfInventory.SetActive(true);
                wantedInventorySystem = "";
                currentSelectedInventorySystem = "shelf";
                break;
            case "slotsdefault":
                transferInventoryMethod(HandInventory);
                HandInventory.SetActive(true);
                wantedInventorySystem = "";
                currentSelectedInventorySystem = "slotsdefault";
                break;
            case "wristbased":
                transferInventoryMethod(WristInventory);
                WristInventory.SetActive(true);
                wantedInventorySystem = "";
                currentSelectedInventorySystem = "wristbased";
                break;
        }

    }


    public void transferInventoryMethod(GameObject invType)
    {


        loltest = invType.GetComponentsInChildren<SlotScriptV3>();

        int yx = 0;

        foreach (SlotScriptV3 xd in loltest)
        {
            yx += 1;
        }

        Debug.Log("TOTAL SLOTS IN INVENTORY SYSTEM " + invType.name + ": " + yx);

        currentInv.SetActive(false);

        if (yx >= tempStorageInvMove.Count)
        {

            Debug.Log(">>>>>:" + yx + " greater than temp storage count");
            for (int i = 0; i < yx+1; i++)
            {
                for (int j = 0; j < tempStorageInvMove.Count; j++)
                {
                    if (invType.transform.GetChild(i).GetComponent<SlotScriptV3>() != null)
                    {   
                        Debug.Log(i+" // "+yx);


                        if (invType.transform.GetChild(i).GetComponent<SlotScriptV3>().itemInSlot == null)
                        {   
                            Debug.Log("INSERTING ITEM into new INVENTORY:"+tempStorageInvMove[j]+" | COUNT: "+j+" out of "+tempStorageInvMove.Count);
                            invType.transform.GetChild(i).GetComponent<SlotScriptV3>().insertItem(tempStorageInvMove[j]);
                            tempStorageInvMove.RemoveAt(j);
                        }

                    }
                    break;
                }
            }


            tempStorageInvMove.Clear();

        }
        else
        {
            Debug.Log("<<<<<:" + yx + " less than temp storage count");
            // for (int i = 0; i < yx; i++)
            // {
            //     if (invType.transform.GetChild(i).GetComponent<SlotScriptV3>() != null)
            //     {
            //          if (invType.transform.GetChild(i).GetComponent<SlotScriptV3>().itemInSlot == null)
            //         {
            //             invType.transform.GetChild(i).GetComponent<SlotScriptV3>().insertItem(tempStorageInvMove[0]);
            //             tempStorageInvMove.RemoveAt(0);
            //             break;
            //         } else {
            //             break;
            //         }
    
            //     }
            // }

            for (int i = 0; i < yx+1; i++)
            {
                for (int j = 0; j < tempStorageInvMove.Count; j++)
                {
                    if (invType.transform.GetChild(i).GetComponent<SlotScriptV3>() != null)
                    {
                        if (invType.transform.GetChild(i).GetComponent<SlotScriptV3>().itemInSlot == null)
                        {   
                            Debug.Log("INSERTING ITEM into new INVENTORY:"+tempStorageInvMove[j]);
                            invType.transform.GetChild(i).GetComponent<SlotScriptV3>().insertItem(tempStorageInvMove[j]);
                            tempStorageInvMove.RemoveAt(j);
                        }

                    }
                    break;
                }
            }

            for (int k = 0; k < tempStorageInvMove.Count; k++)
            {
                invType.transform.GetChild(0).GetComponent<SlotScriptV3>().ManualTriggerReleaseItem(currentInv.transform.GetChild(k).GetComponent<SlotScriptV3>().itemInSlot);
                Debug.Log(tempStorageInvMove[k].name + " expected to drop to ground");
            }

            tempStorageInvMove.Clear();
        }
    }

    public void setInventoryToHands()
    {
        wantedInventorySystem = "slotsdefault";
        changeInventoryStateCalled = true;
    }
    public void setInventoryToShelf()
    {
        wantedInventorySystem = "shelf";
        changeInventoryStateCalled = true;
    }
    public void setInventoryToWrist()
    {
        wantedInventorySystem = "wristbased";
        changeInventoryStateCalled = true;
    }
}
