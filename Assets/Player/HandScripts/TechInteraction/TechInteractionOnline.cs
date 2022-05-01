using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TechInteractionOnline : MonoBehaviourPunCallbacks
{
    public GameObject body, prop, useButton;
    public OnlineController onlineController;

    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.tag == "tech")
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null && prop.transform.parent.tag == "tech") prop = prop.transform.parent.gameObject;

                onlineController.photonView.RPC("Interact", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID);
            }
        }
    }
}
