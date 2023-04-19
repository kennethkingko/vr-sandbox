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

    public void Start()
    {
        actionCollider = gameObject.GetComponent<Collider>();
        interactingObject = null;
        // currentMaterial = gameObject.transform.parent.GetComponent<Renderer>().material;
        completedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
            gameObject.transform.RotateAround(gameObject.transform.position, gameObject.transform.up, angle);
            diffAngle = Mathf.DeltaAngle(rot.eulerAngles.y, yAngle);
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