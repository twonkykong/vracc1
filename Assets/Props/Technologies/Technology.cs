using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Technology : MonoBehaviour
{
    public Animator anim;
    public GameObject generator;
    public LineRenderer liner;
    public HingeJoint hinge;

    public bool isWorking, needGenerator, useMotor, canWire = true;

    public string componentName;

    private void Update()
    {
        if (!canWire) return;
        if (generator != null)
        {
            liner.positionCount = 2;
            liner.SetPositions(new Vector3[2] { transform.position, generator.transform.position });
        }
        else
        {
            liner.positionCount = 0;
            liner.SetPositions(new Vector3[0] { });
        }
    }

    public void Activate(bool value)
    {
        if (componentName != "") (GetComponent(componentName) as MonoBehaviour).enabled = value;
        if (anim != null) anim.enabled = value;
        if (useMotor) hinge.useMotor = value;
        isWorking = value;
    }
}
