using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedback : MonoBehaviour
{
    TwistActionComponent actionComponent;
    GameObject parentObject;
    private ActionBasedController xr;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<TwistActionComponent>();
        xr = (ActionBasedController) GameObject.FindObjectOfType(typeof(ActionBasedController));
    
        actionComponent.Feedback += Haptics;
    }

    void Haptics()
    {
        xr.SendHapticImpulse(0.3f, 0.1f);
    }
}
