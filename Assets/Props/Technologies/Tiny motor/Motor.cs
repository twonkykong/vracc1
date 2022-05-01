using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    public float speed;
    public GameObject motorObj;

    void Update()
    {
        transform.Rotate(0, 0, speed, Space.Self);
        motorObj.transform.Rotate(0, 0, -speed, Space.Self);
    }
}
