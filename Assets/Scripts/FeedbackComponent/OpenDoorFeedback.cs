using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorFeedback : MonoBehaviour
{
    BaseActionComponent actionComponent;
    public GameObject doorObject;
    public AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        actionComponent.Outcome += OpenDoor;
    }

    // Update is called once per frame
    void OpenDoor()
    {
        doorObject.transform.eulerAngles = new Vector3(0,-90,0);
        audioData.Play(0);
    }
}
