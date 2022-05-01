using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleChair : MonoBehaviour
{
    public GameObject vehicle;
    public string componentName;
    public bool sitting;

    public void Press()
    {
        if (vehicle != null && componentName != null)
        {
            (vehicle.GetComponent(componentName) as MonoBehaviour).enabled = !(vehicle.GetComponent(componentName) as MonoBehaviour).enabled;
        }
    }
}
