using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRb : MonoBehaviour
{
    public Transform lowerPoint, upperPoint;

    private void Update()
    {
        Physics.gravity = (upperPoint.position - lowerPoint.position) / transform.localScale.y * 9.81f;
    }
}
