using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhoneVRCameraLook : MonoBehaviour
{
    public GameObject body;


    void Update()
    {
        Vector3 acc = Input.acceleration * 90;

        Input.compass.enabled = true;
        //Quaternion rot = Quaternion.Euler(-acc.z, Input.compass.magneticHeading, -acc.x);
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(0, Input.compass.magneticHeading, 0), 0.2f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-acc.z, body.transform.eulerAngles.y, -acc.x), 0.2f);

        Debug.Log(Input.GetAxis("LeftStickHorizontal"));
    }
}
