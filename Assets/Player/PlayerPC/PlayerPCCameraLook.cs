using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPCCameraLook : MonoBehaviour
{
    public float sensitivity = 5, slerp = 0.5f;
    public GameObject body, arm1, arm2, head, backCam, handCam, toolgunRot;
    float mouseX, mouseY, slerpTime = 1;

    bool thirdPerson;
    public int cam;
    public GameObject[] cams, camHolders;

    Vector3[] camsPos = new Vector3[2] { Vector3.zero, Vector3.zero };

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;

        if (mouseX > 360) mouseX = 1;
        else if (mouseX < 1) mouseX = 360;

        if (mouseY > 90) mouseY = 90;
        else if (mouseY < -90) mouseY = -90;


        if (GetComponent<UsefulStuff>().riding == false)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-mouseY, 0, 0), slerpTime);
            body.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(0, mouseX, 0), slerpTime);
            head.transform.localRotation = transform.localRotation;
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-mouseY, mouseX, 0), slerpTime);
            head.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GetComponent<Camera>().fieldOfView = 25;
            handCam.GetComponent<Camera>().fieldOfView = 25;
            backCam.GetComponent<Camera>().fieldOfView = 25;
            slerpTime = 0.2f;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            GetComponent<Camera>().fieldOfView = 60;
            handCam.GetComponent<Camera>().fieldOfView = 60;
            backCam.GetComponent<Camera>().fieldOfView = 60;
            slerpTime = 1;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            cam += 1;
            if (cam > cams.Length) cam = 0;
            else if (cam < 0) cam = cams.Length;

            if (cam == 0)
            {
                GetComponent<Camera>().enabled = true;
                foreach (GameObject obj in cams) obj.SetActive(false);
            }
            else
            {
                GetComponent<Camera>().enabled = false;

                foreach (GameObject obj in cams) obj.SetActive(false);
                cams[cam-1].SetActive(true);
            }
        }
        foreach (GameObject cam in cams) cam.transform.localPosition = Vector3.zero;

        RaycastHit hit;
        if (Physics.Linecast(transform.position, cams[0].transform.position, out hit))
        {
            cams[0].transform.localPosition = new Vector3(0, 0, Vector3.Distance(camHolders[0].transform.position, hit.point));
        }

        RaycastHit hit1;
        if (Physics.Linecast(transform.position, cams[1].transform.position, out hit1))
        {
            cams[1].transform.localPosition = new Vector3(0, 0, -Vector3.Distance(camHolders[1].transform.position, hit1.point));
        }
    }
}
