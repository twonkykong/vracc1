using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeldingOnline : MonoBehaviourPunCallbacks
{
    public GameObject body, cam, prop1, prop2, toolgun, hands, menu, emptyObj, usefulThings;
    public bool welding, isMenu;
    public Material weldMat, ropeMat;
    public string jointType = "fixed";
    Vector3 difference;

    int layermask = 1 << 11;

    public OnlineController onlineController;

    private void Start()
    {
        layermask = ~layermask;
    }

    public void ChangeJoint(string type)
    {
        jointType = type;
    }

    public void OpenWeldMenu(bool value)
    {
        isMenu = value;
        menu.SetActive(value);

        cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
        
        toolgun.GetComponent<ToolgunControll>().canChange = !value;
        
    }

    public void StartWelding()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            prop1 = hit.collider.gameObject;
            while (prop1.GetComponent<Rigidbody>() == null) prop1 = prop1.transform.parent.gameObject;

            if (jointType == "remove")
            {
                onlineController.photonView.RPC("Weld", RpcTarget.AllBufferedViaServer, prop1.GetPhotonView().ViewID, jointType);
            }
            else
            {
                welding = true;

                usefulThings.SetActive(false);
                toolgun.GetComponent<ToolgunControll>().canChange = false;
                difference = hit.point - prop1.transform.position;
            }

            toolgun.GetComponent<ToolgunControll>().Click();
            
        }
    }

    public void EndWelding()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            prop2 = hit.collider.gameObject;
            while (prop2.GetComponent<Rigidbody>() == null) prop2 = prop2.transform.parent.gameObject;

            bool welded = false;
            foreach (Joint joint in prop2.GetComponents<FixedJoint>())
            {
                if (joint.connectedBody == prop1.GetComponent<Rigidbody>()) welded = true;
            }

            if (!welded)
            {
                GameObject g = PhotonNetwork.Instantiate(emptyObj.name, difference + prop1.transform.position, Quaternion.identity);

                int count = prop1.GetComponents<Joint>().Length;

                GameObject g1 = PhotonNetwork.Instantiate(emptyObj.name, hit.point, Quaternion.identity);
                g1.transform.parent = prop2.transform;

                onlineController.photonView.RPC("Weld", RpcTarget.AllBufferedViaServer, prop1.GetPhotonView().ViewID, jointType, prop2.GetPhotonView().ViewID, g.GetPhotonView().ViewID, g1.GetPhotonView().ViewID);
            }

            
        }

        welding = false;

        usefulThings.SetActive(true);
        toolgun.GetComponent<ToolgunControll>().canChange = true;

        toolgun.GetComponent<ToolgunControll>().Click();
    }
}
