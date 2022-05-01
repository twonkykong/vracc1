using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    public GameObject obj, upperPart;
    public FixedJoint joint;

    private void Update()
    {
        upperPart.transform.localPosition = obj.transform.localPosition;
        joint.connectedAnchor = obj.transform.localPosition;
    }
}
