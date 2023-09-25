using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutcome : MonoBehaviour
{
    BaseActionComponent actionComponent;
    public AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        actionComponent.Outcome += PlayAudio;
    }

    // Update is called once per frame
    void PlayAudio()
    {
        audioData.Play(0);
    }
}
