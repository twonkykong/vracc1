using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObjRB : MonoBehaviour
{
    public GameObject weldedObj;
    Vector3 difference;

    private void Start()
    {
        difference = transform.position - weldedObj.transform.position;
    }
    void Update()
    {
        transform.position = weldedObj.transform.position + difference;
    }
}
