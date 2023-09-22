using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardFeedback : MonoBehaviour
{
    BaseActionComponent actionComponent;
    GameObject parentObject;
    Vector3 parentPosInitial;

    public Vector3 boundsDirection;
    float parentObjectLength;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        parentObject = gameObject.transform.parent.gameObject;
        parentPosInitial = parentObject.transform.position;
        actionComponent.Feedback += MoveForward;

        // to get length of object 
        // if object at an angle, may need to manually input size instead
        if(boundsDirection.x > 0) {
            parentObjectLength = parentObject.GetComponent<Renderer>().bounds.size.x;
        }
        else if(boundsDirection.y > 0) {
            parentObjectLength = parentObject.GetComponent<Renderer>().bounds.size.y;
        }
        else if(boundsDirection.z > 0) {
            parentObjectLength = parentObject.GetComponent<Renderer>().bounds.size.z;
        }
        else {
            Debug.LogError("Must set bounds direction", gameObject.transform);
        }        
        //Debug.Log(gameObject.transform.parent.name + ": " + parentObjectLength);
    }

    void MoveForward()
    {
        float move = parentObjectLength * (actionComponent.totalProgress/actionComponent.requirement);
        Vector3 move3d = transform.forward*move;
        parentObject.transform.position = new Vector3(
        parentPosInitial.x + move3d.x,
        parentPosInitial.y + move3d.y,
        parentPosInitial.z + move3d.z);
    }
}
