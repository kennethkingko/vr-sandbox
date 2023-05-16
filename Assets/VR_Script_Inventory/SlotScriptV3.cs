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

    // current object to certain slot distance 
    float omegalul;

    void Start()
    {
        slotImage = GetComponentInChildren<Image>();
        originalColor = slotImage.color;
        inventoryScript = GameObject.Find("InventoryRuntime").GetComponent<InventoryVR>();
        // device = inventoryScript.subDevice[0];
        lastFrameGrabbed = false;
    }

    void Update()
    {
        // bool xValue;
        // if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out xValue) && xValue)
        // {
        //     Debug.Log("SLOTSCRIPTV3: RH Grip button is pressed.");
        // }

        if (!this.itemInSlot){
            resetColor();
        }
    }


    private void OnTriggerStay(Collider other)
    {

        // Debug.Log("SLOTSCRIPTV3: ONTRIGGERSTAY");
        if (itemInSlot != null)
        {
            // Debug.Log("Item in slot not null");
            return;
        }
        GameObject obj = other.gameObject;
        if (!isItem(obj))
        {
            // Debug.Log("Item not designated an Item");
            return;
        }
        if (obj.GetComponent<ColliderList>().getColliderList.Count > 1)
        {
            // Debug.Log("Collider count issue");

            // return;

            GameObject otherTemp = other.gameObject;
            List<float> otherObjDistanceCollision = new List<float>();

            foreach (GameObject x in obj.GetComponent<ColliderList>().getColliderList)
            {

                if (x == this.gameObject)
                {
                    omegalul = Vector3.Distance(otherTemp.transform.position, x.transform.position);
                }
                else
                {
                    otherObjDistanceCollision.Add(Vector3.Distance(otherTemp.transform.position, x.transform.position));
                }

                Debug.Log("Gameobj " + other.name + " detected collision with multiple slots, one of which is " + x);
                Debug.Log("OBJ to Slot " + x + " Distance:" + (otherObjDistanceCollision));

            }

            foreach (float y in otherObjDistanceCollision)
            {
                if (omegalul > y)
                {
                    return;
                }
            }



        }
        if (!obj.GetComponent<XRItemInteractionScriptV2>().isGrabbing)
        {
            Debug.Log("SLOTSCRIPTV3: Insert Item Condition Fulfilled");
            insertItem(obj);
        }
        // lastFrameGrabbed = obj.GetComponent<XRItemInteractionScriptV2>().isGrabbing;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("SLOTSCRIPTV3: ONTRIGGEREXIT");
        if (itemInSlot != null)
        {
            if (other.gameObject.GetComponent<ItemScriptV3>().inSlot)
            {
                other.gameObject.GetComponent<ItemScriptV3>().currentSlot.itemInSlot = null;
                // other.gameObject.transform.localScale = other.gameObject.GetComponent<ItemScriptV3>().defaultScale;
                // other.gameObject.transform.localScale = other.gameObject.transform.localScale * 2f;
                // other.gameObject.transform.parent = null;

                other.transform.SetParent(null);
                
                other.gameObject.GetComponent<ItemScriptV3>().inSlot = false;
                other.gameObject.GetComponent<ItemScriptV3>().currentSlot.resetColor();
                other.gameObject.GetComponent<ItemScriptV3>().currentSlot = null;
                
                // other.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            }

        }

    }

    bool isItem(GameObject obj)
    {
        return obj.GetComponent<ItemScriptV3>();
    }

    // Update is called once per frame

    public void insertItem(GameObject obj)
    {
        Debug.Log("INSERT ITEM METHOD: Successfully inserted " + obj.gameObject.name);
        // obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        obj.transform.localScale = obj.GetComponent<ItemScriptV3>().defaultScale * 0.5f;
        obj.transform.SetParent(this.gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<ItemScriptV3>().slotRotation;
        obj.GetComponent<ItemScriptV3>().inSlot = true;
        obj.GetComponent<ItemScriptV3>().currentSlot = this;
        itemInSlot = obj;
        slotImage.color = Color.grey;
    }

    public void resetColor()
    {
        slotImage.color = originalColor;
    }


    public void ManualTriggerReleaseItem(GameObject other)
    {
        if (itemInSlot != null)
        {
            if (other.gameObject.GetComponent<ItemScriptV3>().inSlot)
            {
                other.gameObject.GetComponent<ItemScriptV3>().currentSlot.itemInSlot = null;
                other.transform.SetParent(null);
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                
                other.gameObject.GetComponent<ItemScriptV3>().inSlot = false;
                other.gameObject.GetComponent<ItemScriptV3>().currentSlot.resetColor();
                other.gameObject.GetComponent<ItemScriptV3>().currentSlot = null;
                

            }

        }

    }

}
