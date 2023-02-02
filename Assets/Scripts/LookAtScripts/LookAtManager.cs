using UnityEngine;
using System.Collections;

public class LookAtManager : MonoBehaviour
{
    public Camera viewCamera;
    private GameObject lastLookedUpon;

    void Update()
    {
        CheckLookAt();
    }

    private void CheckLookAt()
    {
        if (lastLookedUpon)
        {
            lastLookedUpon.SendMessage("NotLookingUpon", SendMessageOptions.DontRequireReceiver);
        }

        Ray lookAtRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(lookAtRay, out hit, Mathf.Infinity))
        {
            hit.transform.SendMessage("LookingUpon", SendMessageOptions.DontRequireReceiver);
            lastLookedUpon = hit.transform.gameObject;
        }
    }
}