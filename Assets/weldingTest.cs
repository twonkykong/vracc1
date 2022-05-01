using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weldingTest : MonoBehaviour
{
    public Vector3 difference;
    public GameObject weldedObj;

    void Start()
    {
        difference = transform.position - weldedObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = weldedObj.transform.position + difference;
    }
}
