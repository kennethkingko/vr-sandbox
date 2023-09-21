using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFeedbackComponent : MonoBehaviour
{
    BaseActionComponent actionComponent;
    GameObject parentObject;
    
    public AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        actionComponent.Feedback += PlaySound;
    }

    // Update is called once per frame
    void PlaySound()
    {
        audioData.Play(0);
    }
}
