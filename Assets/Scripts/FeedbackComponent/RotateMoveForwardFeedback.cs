using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMoveForwardFeedback : MonoBehaviour
{
    public TwistActionComponent actionComponent;
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

        // object forward movement
        parentObject.transform.position = new Vector3(
        actionComponent.parentPosInitial.x + actionComponent.move3d.x,
        actionComponent.parentPosInitial.y + actionComponent.move3d.y,
        actionComponent.parentPosInitial.z + actionComponent.move3d.z);

        Debug.Log("Feedback: " + actionComponent.move3d + " - " + actionComponent.totalDeltaAngle);
    }
}
