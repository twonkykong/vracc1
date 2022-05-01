using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NPCOnline : MonoBehaviourPun
{
    public string componentName;

    void Start()
    {
        if (!this.photonView.IsMine) (GetComponent(componentName) as MonoBehaviour).enabled = false;
    }
}
