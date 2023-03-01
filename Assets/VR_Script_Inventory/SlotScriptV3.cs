using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SlotScriptV3 : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject itemInSlot;
    public Image slotImage;
    public Color originalColor;
    
    public InputDevice device;

    public InventoryVR inventoryScript;

    bool lastFrameGrabbed;

    void Start()
    {
        slotImage = GetComponentInChildren<Image>();
        originalColor = slotImage.color;
        inventoryScript = GameObject.Find("InventoryRuntime").GetComponent<InventoryVR>();
        // device = inventoryScript.subDevice[0];
        lastFrameGrabbed = false;
    }

    void Update(){
        // bool xValue;
        // if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out xValue) && xValue)
        // {
        //     Debug.Log("SLOTSCRIPTV3: RH Grip button is pressed.");
        // }
    }


    private void OnTriggerStay(Collider other)
    {   

        // Debug.Log("SLOTSCRIPTV3: ONTRIGGERSTAY");
        if (itemInSlot != null) {
            Debug.Log("Item in slot not null");
            return;
        }
        GameObject obj = other.gameObject;
        if (!isItem(obj)) {
            Debug.Log("Item not designated an Item");
            return;
        }
        if (obj.GetComponent<ColliderList>().getColliderList.Count > 1) {
            Debug.Log("Collider count issue");
            return;
        }
        if (!obj.GetComponent<XRItemInteractionScriptV2>().isGrabbing)
        {
            Debug.Log("SLOTSCRIPTV3: Insert Item Condition Fulfilled");
            insertItem(obj);
        }
        // lastFrameGrabbed = obj.GetComponent<XRItemInteractionScriptV2>().isGrabbing;
    }

    private void OnTriggerExit(Collider other){
        Debug.Log("SLOTSCRIPTV3: ONTRIGGEREXIT");
        if (other.gameObject.GetComponent<ItemScriptV3>().inSlot)
        {
            other.gameObject.GetComponent<ItemScriptV3>().currentSlot.itemInSlot = null;
            other.gameObject.transform.parent = null;
            other.gameObject.GetComponent<ItemScriptV3>().inSlot = false;
            other.gameObject.GetComponent<ItemScriptV3>().currentSlot.resetColor();
            other.gameObject.GetComponent<ItemScriptV3>().currentSlot = null;
            // other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
        }
    }

    bool isItem(GameObject obj)
    {
        return obj.GetComponent<ItemScriptV3>();
    }

    // Update is called once per frame

    public void insertItem(GameObject obj){
        Debug.Log("INSERT ITEM METHOD: Successfully inserted "+obj.gameObject.name);
        // obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        obj.transform.SetParent(this.gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<ItemScriptV3>().slotRotation;
        obj.GetComponent<ItemScriptV3>().inSlot = true;
        obj.GetComponent<ItemScriptV3>().currentSlot = this;
        itemInSlot = obj;
        slotImage.color = Color.grey;
    }

    public void resetColor(){
        slotImage.color = originalColor;
    }

}
