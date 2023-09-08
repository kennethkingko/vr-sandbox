using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardFeedback : MonoBehaviour
{
    TwistActionComponent actionComponent;
    GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<TwistActionComponent>();
        parentObject = gameObject.transform.parent.gameObject;

        actionComponent.Feedback += MoveForward;
    }

    void MoveForward()
    {
        parentObject.transform.position = new Vector3(
        actionComponent.parentPosInitial.x + actionComponent.move3d.x,
        actionComponent.parentPosInitial.y + actionComponent.move3d.y,
        actionComponent.parentPosInitial.z + actionComponent.move3d.z);
    }
}
