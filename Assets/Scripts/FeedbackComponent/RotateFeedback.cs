using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFeedback : MonoBehaviour
{
    TwistActionComponent actionComponent;
    GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<TwistActionComponent>();
        parentObject = gameObject.transform.parent.gameObject;

        actionComponent.Feedback += RotateMoveForward;
    }

    void RotateMoveForward()
    {
        // object rotation
        parentObject.transform.eulerAngles = new Vector3(
        parentObject.transform.eulerAngles.x,
        parentObject.transform.eulerAngles.y,
        actionComponent.parentObjAngleInitial + actionComponent.totalDeltaAngle);
    }
}
