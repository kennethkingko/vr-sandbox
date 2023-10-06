using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// The ObjectStateManager is the generic state manager for objects that will interact with action components in the world. Object regarded as tools should utilize this generic state manager to which many other actions can derive from. This state manager can be used as a superclass for other tools that require unique interactions.
/// </summary>
public class ObjectStateManager : MonoBehaviour
{
    // Checking of current state, and changes on materials if necessary, can be changed as needed
    [SerializeField] public ObjectBaseState currentState;
    [System.NonSerialized] public MeshRenderer meshRenderer;
    public Material defaultMat;
    public Material onGrabMat;
    public Material onRaycastMat;

    // Declaration of states
    public ObjectIdleState objectIdleState = new ObjectIdleState();
    public ObjectGrabbedState objectGrabbedState = new ObjectGrabbedState(); 
    public ObjectGrabHoverState objectGrabHoverState = new ObjectGrabHoverState(); 

    // Checking of controls
    public bool isEnabled = true;
    public bool isGrabbed = false;
    public bool isStatic;
    public bool isTriggerOn;

    // Variables for how sensitive this state manager should detect interaction
    public GameObject currentInteractingObject;
    public GameObject raycastOrigin;
    public Vector3 raycastDirection;
    public List<GameObject> colliderObjects;
    public float range;
    public float angle;

    protected XRGrabInteractable interactable;
    
    [SerializeField] protected LayerMask _layerMask;

    TimeSession timer;

    // Generates its own XRGrabInteractable to avoid separate dependency and declaration
    protected virtual void Awake()
    {
        gameObject.AddComponent<XRGrabInteractable>();
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        meshRenderer = GetComponent<MeshRenderer>();
        //Debug.Log(gameObject.name + " - " + meshRenderer);
    }

    protected virtual void Start()
    {
        colliderObjects = new List<GameObject>();
        _layerMask = LayerMask.GetMask("Colliders");
        this.currentState = objectIdleState;
        this.currentState.EnterState(this);
        // this.GetComponent<MeshRenderer>().material = defaultMat;
        timer = GameObject.Find("Timer").GetComponent<TimeSession>();
    }

    void Update()
    {
        this.currentState.UpdateState(this);

        HandleGrabState();
        HandleTrigger();
    }

    // All switch handlings are handled a state manager level
    public void SwitchState(ObjectBaseState state)
    {
        this.currentState = state;
        this.currentState.EnterState(this);
    }

    public void EnableInteractable(bool isEnabled)
    {
        this.isEnabled = isEnabled;
        interactable.enabled = isEnabled;
    }

    // Handler function if the object is grabbeed
    public virtual void HandleGrabState()
    {
        if (interactable.isSelected && this.currentState is ObjectIdleState)
        {
            EnterGrabbedState();
            timer.StartWatch();
        }
        if (!interactable.isSelected)
        {
            ExitGrabbedState();
        }
    } 

    public void EnterGrabbedState()
    {
        this.SwitchState(objectGrabbedState);
    }

    public void ExitGrabbedState()
    {
        this.SwitchState(objectIdleState);
    }

    // Handle function to check if the trigger button on controllers are held
    public void HandleTrigger()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand, devices);
        
        foreach (var device in devices)
        {
            bool featureValue;
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out featureValue) && featureValue)
            {
                //Debug.Log("Trigger on!");
                this.isTriggerOn = true;
            }
            else
            {
                this.isTriggerOn = false;
            }
        }   
        
    }

    public void TriggerPressed()
    {
        this.isTriggerOn = true;
    }

    public void TriggerReleased()
    {
        this.isTriggerOn = false;
    }

    // Customizable object check whether this object is interacting with the corresponding action component
    public bool IsObjectCorrect(string colliderName)
    {
        // Add the name of this osm's gameobject to the intended receiver objects
        Debug.Log(colliderName + " - " + this.transform.name);
        if(colliderName.Contains(this.transform.name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Mathematical function check whether this object is within the angle of interaction
    public bool IsHitObjectWithinAngle(RaycastHit hit, Vector3 start, Vector3 end, float theta)
    {
        float deg = Vector3.Angle(hit.transform.position - start, end - start);

        if (deg <= theta)
        {
            return true;
        }
        return false;
    }

    // Mathematical function check whether this object is within the range of interaction
    public bool IsObjectWithinDistance(RaycastHit hit, float distance)
    {
        if (hit.distance <= distance)
        {
            return true;
        }
        return false;
    }

    // Collision implementation of this object and another object, returns true if there is (a) there is collision, (b) it is not hitting itself, (c) the boolean checks are true, and (d) hitting the correct tags (colliders)
    public bool EmitRay()
    {
        RaycastHit hit;
        bool isHitting;
        Vector3 start, end;

        start = this.raycastOrigin.transform.position;
        end = this.raycastOrigin.transform.position + (this.raycastOrigin.transform.rotation * (this.raycastDirection * range));

        isHitting = Physics.Linecast(start, end, out hit, _layerMask);
        Debug.DrawLine(start, end, Color.green);

        if (isHitting && hit.transform.name != this.transform.name && IsObjectWithinDistance(hit, range) && IsHitObjectWithinAngle(hit, start, end, angle) && IsObjectCorrect(hit.transform.name) && hit.transform.tag == "Colliders")
        {
            
            float deg = Vector3.Angle(hit.transform.position - start, end - start);
            Debug.Log(this.transform.name + " hits: " + hit.transform.name + "(" + hit.distance + ", " + deg + ") :: " + this.raycastOrigin.transform.position + (this.raycastDirection * range));
            currentInteractingObject = hit.transform.gameObject;
            return true;
        }
        currentInteractingObject = null;
        return false;
    }
}
