using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWhenLookingAtProp : MonoBehaviour
{
    public string layer, tag;
    public GameObject[] show;
    int layermask = 1 << 4;

    private void Start()
    {
        layermask = ~layermask;
    }

    private void Update()
    {
        foreach (GameObject obj in show) obj.SetActive(false);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 20f, layermask))
        {
            if (layer == "" || hit.collider.gameObject.layer == Int32.Parse(layer))
            {
                if (hit.collider.tag == tag || tag == "")
                {
                    foreach (GameObject obj in show) obj.SetActive(true);
                }
            }
        }
    }
}
