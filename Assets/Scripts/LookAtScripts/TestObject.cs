using UnityEngine;
using System.Collections;

public class TestObject : MonoBehaviour, LookAtReceiver
{
    private bool isLookedUpon;
    public Material onLookMat;
    public Material defaultMat;

    void Update()
    {
        if (isLookedUpon)
        {
            this.GetComponent<MeshRenderer>().material = onLookMat;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = defaultMat;
        }
    }

    public void LookingUpon()
    {
        isLookedUpon = true;
    }

    public void NotLookingUpon()
    {
        isLookedUpon = false;
    }
}