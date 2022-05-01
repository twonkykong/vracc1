using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float force = 1;

    private void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<Rigidbody>() != null && col.transform.parent != transform) col.GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }

    private void OnEnable()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
    
    private void OnDisable()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
}
