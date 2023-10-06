using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTimerOutcome : MonoBehaviour
{
    BaseActionComponent actionComponent;
    TimeSession timer;

    // Start is called before the first frame update
    void Start()
    {
        actionComponent = gameObject.GetComponent<BaseActionComponent>();
        actionComponent.Outcome += StopTimer;
        timer = GameObject.Find("Timer").GetComponent<TimeSession>();
    }

    void StopTimer()
    {
        timer.StopWatch();
    }
}
