using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedback : MonoBehaviour
{
    BaseActionComponent actionComponent;
    GameObject parentObject;
    private ActionBasedController xr;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        xr = (ActionBasedController) GameObject.FindObjectOfType(typeof(ActionBasedController));
    
        actionComponent.Feedback += Haptics;
    }

    void Haptics()
    {
        xr.SendHapticImpulse(0.3f, 0.1f);
    }
}
