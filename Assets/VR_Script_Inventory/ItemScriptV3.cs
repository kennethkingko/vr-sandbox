using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScriptV3 : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool inSlot;
    public Vector3 slotRotation = Vector3.zero;

    public Vector3 defaultScale;
    public SlotScriptV3 currentSlot;
    

    void Start()
    {
        defaultScale = transform.localScale;
    }

    void FixedUpdate(){
        if (!inSlot){
            this.transform.localScale = defaultScale;
        }
    }


}

