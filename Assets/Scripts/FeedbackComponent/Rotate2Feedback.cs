using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2Feedback : MonoBehaviour
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

        zAngle = parentObjAngleInitial + actionComponent.totalProgress;

        // object rotation
        parentObject.transform.eulerAngles = new Vector3(
        parentObject.transform.eulerAngles.x,
        parentObject.transform.eulerAngles.y,
        zAngle);
    }
}
