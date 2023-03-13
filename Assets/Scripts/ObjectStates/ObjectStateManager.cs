using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectStateManager : MonoBehaviour
{
    public ObjectBaseState currentState;
    public Material defaultMat;
    public Material onGrabMat;
    public Material onRaycastMat;

    public ObjectIdleState objectIdleState = new ObjectIdleState();
    public ObjectGrabbedState objectGrabbedState = new ObjectGrabbedState(); 
    public ObjectGrabHoverState objectGrabHoverState = new ObjectGrabHoverState(); 

    public bool isGrabbed = false;
    public bool isStatic;
    // public XRGrabInteractable interactor = null;

    public GameObject raycastOrigin;
    public Vector3 raycastDirection;
    public float range;
    public float angle;
    
    int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = ~layerMask;
        this.currentState = objectIdleState;
        this.currentState.EnterState(this);
        this.GetComponent<MeshRenderer>().material = defaultMat;
    }

    // private void Awake()
    // {
    //     interactor = GetComponent<XRGrabInteractable>();
    // }

    // Update is called once per frame
    void Update()
    {
        this.currentState.UpdateState(this);
    }

    public void SwitchState(ObjectBaseState state)
    {
        this.currentState = state;
        this.currentState.EnterState(this);
    }

    public void EnterGrabbedState()
    {
        this.SwitchState(objectGrabbedState);
    }

    public void ExitGrabbedState()
    {
        this.SwitchState(objectIdleState);
    }

    public bool IsHitObjectWithinAngle(RaycastHit hit, float theta)
    {
        float deg = Vector3.Angle(hit.transform.position - this.raycastOrigin.transform.position, this.raycastOrigin.transform.position + (this.raycastDirection * range));

        if (deg <= theta)
        {
            return true;
        }
        return false;
    }

    public bool IsObjectWithinDistance(RaycastHit hit, float distance)
    {
        if (hit.distance <= distance)
        {
            return true;
        }
        return false;
    }

    public bool EmitRay()
    {
        RaycastHit hit;
        bool isHitting;

        isHitting = Physics.Linecast(this.raycastOrigin.transform.position, this.raycastOrigin.transform.position + (this.raycastDirection * range), out hit);

        if (isHitting && hit.transform.name != this.transform.name && IsObjectWithinDistance(hit, range) && IsHitObjectWithinAngle(hit, angle))
        // if (isHitting && hit.transform.name != this.transform.name)
        {
            float deg = Vector3.Angle(this.raycastOrigin.transform.position + (this.raycastDirection * range), hit.transform.position - this.raycastOrigin.transform.position);
            Debug.Log(this.transform.name + " hits: " + hit.transform.name + "(" + hit.distance + ", " + deg + ") ::" + this.raycastOrigin.transform.position + (this.raycastDirection * range));
            return true;
            Debug.Log(hit.transform.tag);
        }
        return false;
    }


}
