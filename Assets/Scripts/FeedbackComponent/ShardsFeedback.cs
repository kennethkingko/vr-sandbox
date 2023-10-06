using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardsFeedback : MonoBehaviour
{
    BaseActionComponent actionComponent;
    GameObject parentObject;
    
    public ParticleSystem particleSys;
    public float emissionMin;
    public float emissionMax;
    float emissionRate;
    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        parentObject = gameObject.transform.parent.gameObject;
        actionComponent.Feedback += ShowShards;
    }

    // Update is called once per frame
    void ShowShards()
    {
        emissionRate = emissionMin + ((emissionMax-emissionMin)*(actionComponent.currentProgress/(actionComponent.requirement/2)));
        Debug.Log("emmision: " + emissionRate);
        var emission = particleSys.emission;
        emission.rateOverTime = emissionRate;
        particleSys.Play();
    }
}
