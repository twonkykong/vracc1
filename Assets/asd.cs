using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class asd : MonoBehaviour
{
    public Camera left, right;
    void Start()
    {
        left.stereoTargetEye = StereoTargetEyeMask.Left;
        right.stereoTargetEye = StereoTargetEyeMask.Right;

        XRSettings.enabled = true;
    }
}
