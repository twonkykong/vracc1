using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
#pragma warning disable CS0618 // Тип или член устарел

public class HolderOnline: MonoBehaviourPun
{
    public GameObject body, holdPoint, prop, toolgun, hands, cam, emptyObj, liner;
    public bool holding;

    Vector3 FirstPoint;
    Vector3 SecondPoint;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;
    int touchid;

    public GameObject[] show, hide;
    public OnlineController onlineController;
    public DrawingLine[] liners;
    int layermask = 1 << 4;
    public Toggle fixRot;

    private void Start()
    {
        holdPoint.GetComponent<Rigidbody>().solverIterations = 60;
        layermask = ~layermask;
    }

    IEnumerator update()
    {
        while (true)
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
            yield return new WaitForEndOfFrame();
        }
    }

    public void Hold()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop = hit.collider.gameObject;
                Debug.Log(prop.name);
                while (prop.GetComponent<Rigidbody>() == null && prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                emptyObj.transform.position = hit.point;
                emptyObj.transform.parent = prop.transform;

                holdPoint.transform.position = hit.point;

                liners[0].enabled =  true;

                onlineController.photonView.RPC("Hold",
                    RpcTarget.AllBufferedViaServer,
                    this.photonView.ViewID,
                    prop.GetPhotonView().ViewID,
                    holdPoint.GetPhotonView().ViewID, 
                    liners[1].gameObject.GetPhotonView().ViewID);

                prop.GetComponent<Rigidbody>().isKinematic = false;
                holding = true;
                toolgun.GetComponent<ToolgunControll>().Hold(true);

                foreach (GameObject obj in toolgun.GetComponent<ToolgunControll>().weapons) obj.SetActive(false);

                if (show != null && hide != null)
                {
                    foreach (GameObject obj in show) obj.SetActive(true);
                    foreach (GameObject obj in hide) obj.SetActive(false);
                }
            }
        }
    }

    public void Drop()
    {
        onlineController.photonView.RPC("Drop", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, holdPoint.GetPhotonView().ViewID, liners[1].gameObject.GetPhotonView().ViewID);
        holding = false;
        toolgun.GetComponent<ToolgunControll>().Hold(false);
        prop.GetComponent<Rigidbody>().AddForce(Vector3.up * -0.01f, ForceMode.Impulse);
        emptyObj.transform.parent = null;
        liners[0].enabled = false;

        toolgun.GetComponent<ToolgunControll>().weapons[toolgun.GetComponent<ToolgunControll>().weaponNumber].SetActive(true);

        if (show != null && hide != null)
        {
            foreach (GameObject obj in show) obj.SetActive(false);
            foreach (GameObject obj in hide) obj.SetActive(true);
        }
    }

    public void Throw()
    {
        onlineController.photonView.RPC("Drop", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, holdPoint.GetPhotonView().ViewID, liners[1].gameObject.GetPhotonView().ViewID);
        prop.GetComponent<Rigidbody>().AddForce(Vector3.up * -0.01f, ForceMode.Impulse);
        prop.GetComponent<Rigidbody>().AddForce(transform.forward * 4f * prop.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
        //toolgun.GetComponent<HandControll>().canChange = true;
        toolgun.GetComponent<ToolgunControll>().Hold(false);
        toolgun.GetComponent<ToolgunControll>().Click();
        emptyObj.transform.parent = null;
        liners[0].enabled = false;

        toolgun.GetComponent<ToolgunControll>().weapons[toolgun.GetComponent<ToolgunControll>().weaponNumber].SetActive(true);

        if (show != null && hide != null)
        {
            foreach (GameObject obj in show) obj.SetActive(false);
            foreach (GameObject obj in hide) obj.SetActive(true);
        }
    }

    public void Fix()
    {
        onlineController.photonView.RPC("Drop", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, holdPoint.GetPhotonView().ViewID, liners[1].gameObject.GetPhotonView().ViewID);
        onlineController.photonView.RPC("Fix", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID);
        toolgun.GetComponent<ToolgunControll>().Hold(false);
        emptyObj.transform.parent = null;
        liners[0].enabled = false;

        toolgun.GetComponent<ToolgunControll>().weapons[toolgun.GetComponent<ToolgunControll>().weaponNumber].SetActive(true);

        if (show != null && hide != null)
        {
            foreach (GameObject obj in show) obj.SetActive(false);
            foreach (GameObject obj in hide) obj.SetActive(true);
        }
    }

    public void EnableRotation()
    {
        if (fixRot.isOn)
        {
            xAngle = holdPoint.transform.eulerAngles.y;
            yAngle = -holdPoint.transform.eulerAngles.x;

            StartCoroutine(update());
        }
        else StopCoroutine(update());

        cam.GetComponent<PlayerPhoneCameraLook>().enabled = !fixRot.isOn;
        
    }

    public void ZoomObject(float value)
    {
        if (value == 1)
        {
            onlineController.photonView.RPC("ChangeAnchor", RpcTarget.AllBufferedViaServer, this.photonView.ViewID, prop.GetPhotonView().ViewID, holdPoint.GetPhotonView().ViewID, 1);
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
                onlineController.photonView.RPC("ChangeAnchor", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, holdPoint.GetPhotonView().ViewID, -1);
            }
        }
    }
}
