using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassTwoLeverTorqueActionComponent : BaseActionComponent
{
    [SerializeField] GameObject interactingObject;
    public float distance;
    public float requiredAngle;
    public float thresholdAngle;
    [SerializeField] float angle;
    float yAngle, diffAngle;
    Material currentMaterial;
    Color completedColor;
    bool isTurning = false;
    float parentZ;

    public void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
        // currentMaterial = gameObject.transform.parent.GetComponent<Renderer>().material;
        completedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        GameObject parentObject = gameObject.transform.parent.gameObject;
        parentZ = gameObject.transform.parent.gameObject.transform.eulerAngles.z;
    }
    
    public override void Update()
    {
        if (!isCompleted && interactingObject != null) CheckIfCompleted();
    }

    public override void OnEntry(GameObject go)
    {
        if (go != null)
        {
            interactingObject = go;
            Vector3 pos = go.transform.position;
            Quaternion rot = go.transform.rotation;
            yAngle = rot.eulerAngles.y;
            diffAngle = 0.0f;
            Debug.Log("Entry transform: " + pos + " " + rot);
        }
    }

    public bool CheckAction()
    {
        bool distanceCheck = false;

        distanceCheck = Vector3.Distance(interactingObject.transform.position, gameObject.transform.position) <= distance;

        return distanceCheck;
    }

    public override void CheckIfCompleted()
    {   
        Debug.Log("Check if completed");
        if (interactingObject.GetComponent<ObjectStateManager>().currentState is ObjectGrabHoverState)
        {
            Vector3 pos = interactingObject.transform.position;
            Quaternion rot = interactingObject.transform.rotation;
            diffAngle = Mathf.DeltaAngle(rot.eulerAngles.y, yAngle);            
                        
            GameObject parentObject = gameObject.transform.parent.gameObject;
            //parentObject.transform.RotateAround(parentObject.transform.position, parentObject.transform.forward, diffAngle);
            // parentObject.transform.Rotate(0, 0, -diffAngle, Space.Self);
            Debug.Log("diffAngle: " + diffAngle);
            Debug.Log("change: " + Mathf.Abs((parentZ-diffAngle) - parentObject.transform.eulerAngles.z));

            if (Mathf.Abs((parentZ-diffAngle) - parentObject.transform.eulerAngles.z)> 3) {
                parentObject.transform.eulerAngles = new Vector3(
                parentObject.transform.eulerAngles.x,
                parentObject.transform.eulerAngles.y,
                parentZ-diffAngle);
                Debug.Log("parentObject: " + parentObject.transform.eulerAngles.y);
            }           
            
            // currentMaterial.color = Color.Lerp(currentMaterial.color, completedColor, angle / requiredAngle);
            // Debug.Log("Current color: " + currentMaterial.color);
            Debug.Log("Current angle: (" + angle +")");
        }
        else
        {
            angle += diffAngle;
            interactingObject = null;
            isTurning = false;
        }

        if (angle >= requiredAngle)
        {
            isCompleted = true;
            Debug.Log("Turning action completed on " + gameObject.transform.parent.name);
        }
    }
}