using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChangeSizeOnline : MonoBehaviourPun
{
    public GameObject body, prop, toolgun, hands, cords;
    public bool x, y, z;
    public Toggle xToggle, yToggle, zToggle, wholeObject;

    public OnlineController onlineController;
    int layermask = 1 << 4;

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
                cords.transform.rotation = prop.transform.rotation;
            }
        }
    }

    public void IncreaseSize()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                if (!xToggle.isOn && !yToggle.isOn && !zToggle.isOn) onlineController.photonView.RPC("ChangeSize", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, 1.1f, 1.1f, 1.1f, true, wholeObject.isOn);
                else
                {
                    float xSize = 1;
                    float ySize = 1;
                    float zSize = 1;

                    if (xToggle.isOn == true) xSize = 1.1f;
                    if (yToggle.isOn == true) ySize = 1.1f;
                    if (zToggle.isOn == true) zSize = 1.1f;

                    onlineController.photonView.RPC("ChangeSize", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, xSize, ySize, zSize, true, wholeObject.isOn);
                }

                toolgun.GetComponent<ToolgunControll>().Click();

            }
        }
    }

    public void ReduceSize()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                if (!xToggle.isOn && !yToggle.isOn && !zToggle.isOn) onlineController.photonView.RPC("ChangeSize", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, 1.1f, 1.1f, 1.1f, false, wholeObject.isOn);
                else
                {
                    float xSize = 1;
                    float ySize = 1;
                    float zSize = 1;

                    if (xToggle.isOn == true) xSize = 1.1f;
                    if (yToggle.isOn == true) ySize = 1.1f;
                    if (zToggle.isOn == true) zSize = 1.1f;

                    onlineController.photonView.RPC("ChangeSize", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, xSize, ySize, zSize, false, wholeObject.isOn);
                }

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }
}
