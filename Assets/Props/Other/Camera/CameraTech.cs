using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraTech : MonoBehaviour
{
    public List<Camera> cams;
    private void OnEnable()
    {
        cams = FindObjectsOfType<Camera>().ToList();
        foreach (Camera cam in cams.ToList())
        {
            if (cam.enabled == false) cams.Remove(cam);
        }

        foreach (Camera cam in cams) cam.enabled = false;
        GetComponent<Camera>().enabled = true;
    }

    private void OnDisable()
    {
        foreach (Camera cam in cams) cam.enabled = true;
        GetComponent<Camera>().enabled = false;
    }

    private void OnDestroy()
    {
        foreach (Camera cam in cams) cam.enabled = true;
        GetComponent<Camera>().enabled = false;
    }
}
