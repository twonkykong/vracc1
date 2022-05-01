using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChangeWeightOnline : MonoBehaviourPun
{
    public GameObject body, prop, toolgun;
    public Text currentMass;
    int layermask = 1 << 4;

    public OnlineController onlineController;

    private void Start()
    {
        layermask = ~layermask;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop = hit.collider.gameObject;
                while (prop.GetComponent<Rigidbody>() == null) prop = prop.transform.parent.gameObject;

                currentMass.text = "mass: " + System.Math.Round(prop.GetComponent<Rigidbody>().mass, 1);
                if (prop.GetComponent<Rigidbody>().useGravity == false) currentMass.text = "gravity off";
            }
            
        }
    }

    public void IncreaseWeight()
    {
        onlineController.photonView.RPC("ChangeWeight", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, 1.1f, true);
        toolgun.GetComponent<ToolgunControll>().Click();
    }

    public void ReduceWeight()
    {
        onlineController.photonView.RPC("ChangeWeight", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, 1.1f, false);
        toolgun.GetComponent<ToolgunControll>().Click();
    }

    public void SwitchGravity()
    {
        onlineController.photonView.RPC("SwitchGravity", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, prop.GetComponent<Rigidbody>().useGravity);
        toolgun.GetComponent<ToolgunControll>().Click();
    }
}
