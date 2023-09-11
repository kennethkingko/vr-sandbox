using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFeedback : MonoBehaviour
{
    BaseActionComponent actionComponent;
    GameObject parentObject;
    float parentObjAngleInitial;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        parentObject = gameObject.transform.parent.gameObject;
        parentObjAngleInitial = parentObject.transform.eulerAngles.z;
        actionComponent.Feedback += RotateMoveForward;
    }

    void RotateMoveForward()
    {
        // object rotation
        parentObject.transform.eulerAngles = new Vector3(
        parentObject.transform.eulerAngles.x,
        parentObject.transform.eulerAngles.y,
        parentObjAngleInitial - (actionComponent.percentageCompleted*actionComponent.requirement));
    }
}
