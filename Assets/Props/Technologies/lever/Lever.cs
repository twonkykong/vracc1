using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Animation anim;
    public bool isWorking;
    public GameObject obj, handle;
    public FixedJoint joint;

    public void Switch()
    {
        isWorking = !isWorking;
        if (isWorking) anim.Play("on");
        else anim.Play("off");

        GetComponent<PowerGenerator>().isWorking = isWorking;
    }

    private void Update()
    {
        handle.transform.localPosition = obj.transform.localPosition;
        handle.transform.localRotation = obj.transform.localRotation;
        joint.connectedAnchor = obj.transform.localPosition;
    }
}
