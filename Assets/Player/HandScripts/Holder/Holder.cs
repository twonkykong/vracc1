using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS0618 // Тип или член устарел

public class Holder : MonoBehaviour
{
    public GameObject body, holdPoint, propPoint, prop, toolgun, hands, cam;
    public bool holding, rotating;
    float bodyRot;
    public DrawingLine[] liners;

    Vector3 FirstPoint;
    Vector3 SecondPoint;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;
    int touchid;
    public Toggle fixRotToggle;

    public GameObject[] show, hide;

    Quaternion propRot;

    private void Start()
    {
        holdPoint.GetComponent<Rigidbody>().solverIterations = 60;
    }

    private void Update()
    {
        if (!holding)
        {
            if (body.GetComponent<Player>().mode == "pc")
            {
                if (Input.GetKeyDown(KeyCode.E)) Hold();
            }
        }
        else
        {
            if (fixRotToggle.isOn) holdPoint.transform.rotation = propRot;
            if (body.GetComponent<Player>().mode == "pc")
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0) ZoomObject(Input.GetAxis("Mouse ScrollWheel"));
            }

            if (!rotating)
            {
                if (body.GetComponent<Player>().mode == "pc")
                {
                    if (Input.GetKeyDown(KeyCode.E)) Drop();
                    if (Input.GetMouseButtonDown(0)) Throw();
                    if (Input.GetMouseButtonDown(1)) Fix();
                }
            }
            else
            {
                if (body.GetComponent<Player>().mode == "pc")
                {
                    holdPoint.transform.RotateAround(transform.right, Input.GetAxis("Mouse Y") / 10);
                    holdPoint.transform.RotateAround(transform.up, -Input.GetAxis("Mouse X") / 10);
                }
                
                else if (body.GetComponent<Player>().mode == "phone")
                {
                    if (Input.touchCount > 0)
                    {
                        if (Input.touchCount > 1)
                        {
                            foreach (Touch touch in Input.touches) if (touch.position.x > Screen.width / 2) touchid = touch.fingerId;
                        }
                        else touchid = 0;

                        if (Input.GetTouch(touchid).position.x > Screen.width / 2)
                        {
                            if (Input.GetTouch(touchid).phase == TouchPhase.Began)
                            {
                                FirstPoint = Input.GetTouch(touchid).position;
                                xAngleTemp = xAngle;
                                yAngleTemp = yAngle;
                            }
                            if (Input.GetTouch(touchid).phase == TouchPhase.Moved)
                            {
                                if (yAngle >= 89.9f) yAngle = 89.9f;
                                if (yAngle <= -89.9f) yAngle = -89.9f;
                                if (xAngle >= 360 || xAngle <= -360) xAngle = 0;

                                SecondPoint = Input.GetTouch(touchid).position;
                                xAngle = xAngleTemp + (SecondPoint.x - FirstPoint.x) * 180 * cam.GetComponent<PlayerPhoneCameraLook>().sensitivity / Screen.width;
                                yAngle = yAngleTemp + (SecondPoint.y - FirstPoint.y) * 90 * cam.GetComponent<PlayerPhoneCameraLook>().sensitivity / Screen.height;
                            }
                        }
                    }

                    holdPoint.transform.rotation = Quaternion.Euler(-yAngle, xAngle, 0);
                }
            }

            if (body.GetComponent<Player>().mode == "pc")
            {
                if (Input.GetKeyDown(KeyCode.R)) EnableRotation(true);
                if (Input.GetKeyUp(KeyCode.R)) EnableRotation(false);
            }
        }
    }

    public void Hold()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null && prop.transform.parent.GetComponent<Rigidbody>() != null) prop = prop.transform.parent.gameObject;
                propRot = prop.transform.rotation;
                ToggleFixRot();
                propPoint.transform.position = hit.point;
                propPoint.transform.parent = prop.transform;

                holdPoint.transform.position = hit.point;
                holdPoint.GetComponent<FixedJoint>().connectedBody = prop.GetComponent<Rigidbody>();
                prop.GetComponent<Rigidbody>().isKinematic = false;
                holding = true;
                toolgun.GetComponent<ToolgunControll>().Hold(true);

                foreach (GameObject obj in toolgun.GetComponent<ToolgunControll>().weapons) obj.SetActive(false);

                if (show != null && hide != null)
                {
                    foreach (GameObject obj in show) obj.SetActive(true);
                    foreach (GameObject obj in hide) obj.SetActive(false);
                }

                prop.GetComponent<Outline>().enabled = true;
                foreach (DrawingLine liner in liners) liner.enabled = true;
            }
        }
    }

    public void Drop()
    {
        foreach (DrawingLine liner in liners) liner.enabled = false;
        holdPoint.GetComponent<FixedJoint>().connectedBody = null;
        holding = false;
        toolgun.GetComponent<ToolgunControll>().Hold(false);

        toolgun.GetComponent<ToolgunControll>().weapons[toolgun.GetComponent<ToolgunControll>().weaponNumber].SetActive(true);

        if (show != null && hide != null)
        {
            foreach (GameObject obj in show) obj.SetActive(false);
            foreach (GameObject obj in hide) obj.SetActive(true);
        }
        prop.GetComponent<Outline>().enabled = false;
        propPoint.transform.parent = null;
    }

    public void Throw()
    {
        foreach (DrawingLine liner in liners) liner.enabled = false;
        Drop();
        prop.GetComponent<Rigidbody>().AddForce(transform.forward * 4f * prop.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
        //toolgun.GetComponent<HandControll>().canChange = true;
        toolgun.GetComponent<ToolgunControll>().Click();
        prop.GetComponent<Outline>().enabled = false;
        propPoint.transform.parent = null;
    }

    public void Fix()
    {
        foreach (DrawingLine liner in liners) liner.enabled = false;
        Drop();
        prop.GetComponent<Rigidbody>().isKinematic = true;
        prop.GetComponent<Outline>().enabled = false;
        propPoint.transform.parent = null;
    }

    public void EnableRotation(bool value)
    {
        if (body.GetComponent<Player>().mode == "pc") cam.GetComponent<PlayerPCCameraLook>().enabled = !value;
        else if (body.GetComponent<Player>().mode == "phone")
        {
            if (value)
            {
                xAngle = holdPoint.transform.eulerAngles.y;
                yAngle = -holdPoint.transform.eulerAngles.x;
            }

            cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
        }
        rotating = value;
    }

    public void ZoomObject(float value)
    {
        if (value == 1)
        {
            prop.transform.position += transform.forward * 0.1f;
            holdPoint.GetComponent<FixedJoint>().connectedAnchor = prop.transform.position;
        }
        else
        {
            Collider col;
            Vector3 point1 = transform.parent.gameObject.GetComponentInParent<Collider>().ClosestPoint(prop.transform.position);
            if (prop.transform.parent != null)
            {
                while (prop.GetComponent<Collider>() == null) prop = prop.transform.parent.gameObject;
                col = prop.GetComponent<Collider>();
            }
            else
            {
                col = prop.GetComponentsInChildren<Collider>()[0];
            }

            Vector3 point2 = col.ClosestPoint(GetComponentInParent<Collider>().ClosestPoint(prop.transform.position));
            if (Vector3.Distance(point1, point2) > 0.5f)
            {
                prop.transform.position -= transform.forward * 0.1f;
                holdPoint.GetComponent<FixedJoint>().connectedAnchor = prop.transform.position;
            }
        }
    }

    public void ToggleFixRot()
    {
        propRot = holdPoint.transform.rotation;
    }
}
