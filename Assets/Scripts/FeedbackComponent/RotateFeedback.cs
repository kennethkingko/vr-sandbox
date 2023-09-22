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
        actionComponent.Feedback += Rotate;
    }

    void Rotate()
    {
        float zAngle;

        if ((actionComponent.requirement < 0 && actionComponent.totalProgress <= actionComponent.requirement) || (actionComponent.requirement >= 0 && actionComponent.totalProgress >= actionComponent.requirement))
        {
            zAngle = parentObjAngleInitial + actionComponent.requirement;
        }
        else
        {
            zAngle = parentObjAngleInitial + actionComponent.totalProgress;
        }

        // object rotation
        parentObject.transform.eulerAngles = new Vector3(
        parentObject.transform.eulerAngles.x,
        parentObject.transform.eulerAngles.y,
        zAngle);
    }
}
