using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WireOnline : MonoBehaviourPun
{
    public GameObject body, prop1, prop2, toolgun, cam, usefulThings;
    public bool wiring, generatorFirst;

    public OnlineController onlineController;

    public void StartWiring()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tech"))
            {
                prop1 = hit.collider.gameObject;
                while (prop1.transform.parent != null && prop1.transform.parent.CompareTag("tech")) prop1 = prop1.transform.parent.gameObject;

                wiring = true;
                toolgun.GetComponent<ToolgunControll>().canChange = false;
                usefulThings.SetActive(false);


                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void EndWiring()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tech"))
            {
                prop2 = hit.collider.gameObject;
                while (prop2.transform.parent != null && prop2.transform.parent.CompareTag("tech")) prop2 = prop2.transform.parent.gameObject;

                Debug.Log("exists : " + onlineController);
                onlineController.photonView.RPC("Wire", RpcTarget.AllBufferedViaServer, prop1.GetPhotonView().ViewID, prop2.GetPhotonView().ViewID);

                wiring = false;
                toolgun.GetComponent<ToolgunControll>().canChange = true;
                usefulThings.SetActive(true);

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void RemoveWire()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tech"))
            {
                prop1 = hit.collider.gameObject;
                while (prop1.transform.parent != null && prop1.transform.parent.CompareTag("tech")) prop1 = prop1.transform.parent.gameObject;

                onlineController.photonView.RPC("RemoveWire", RpcTarget.AllBufferedViaServer, prop1.GetPhotonView().ViewID);
            }
        }
    }
}
