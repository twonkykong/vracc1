using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    public GameObject outterCircle, innerCircle;
    public float slerpTime = 0.5f;

    private void Update()
    {
        Debug.Log(transform.rotation.x + " / " + transform.eulerAngles.x);
    }
}
