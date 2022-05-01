using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool isWorking;
    public Animation anim;

    public GameObject child;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == child) return;
        anim.Play("on");
        isWorking = true;
        GetComponent<PowerGenerator>().isWorking = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == child) return;
        anim.Play("off");
        isWorking = false;
        GetComponent<PowerGenerator>().isWorking = false;
    }
}
