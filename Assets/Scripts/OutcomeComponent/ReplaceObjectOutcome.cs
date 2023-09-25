using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceObjectOutcome : MonoBehaviour
{
    // Start is called before the first frame update
    BaseActionComponent actionComponent;
    GameObject parentObject;
    public GameObject newObject;
    public AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        parentObject = gameObject.transform.parent.gameObject;
        actionComponent.Outcome += ReplaceObject;
    }
    void ReplaceObject()
    {
        audioData.Play(0);
        Instantiate(newObject, parentObject.transform.position, parentObject.transform.rotation);
        Destroy(parentObject);
    }
}
