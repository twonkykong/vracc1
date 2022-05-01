using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingLine : MonoBehaviour
{
    public Transform lineTo;
    public LineRenderer liner;

    private void Update()
    {
        liner.SetPositions(new Vector3[2] { transform.position, lineTo.position });
    }

    private void OnDisable()
    {
        liner.positionCount = 0;
    }

    private void OnEnable()
    {
        liner.positionCount = 2;
    }
}
