using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;

public class YoutubeTVRemoteOnline : MonoBehaviourPun
{
    public InputField text;
    public GameObject prop, cam, body, toolgun, menu;
    public OnlineController onlineController;

    public void OpenMenu(bool value)
    {
        if (value)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
            {
                if (hit.collider.CompareTag("tv"))
                {
                    menu.SetActive(value);
                    cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
                    
                }
            }
        }
        else
        {
            menu.SetActive(value);
            cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
        }
    }

    public void ChangeVideo()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tv"))
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                onlineController.photonView.RPC("ChangeVideo", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, text.text);
                OpenMenu(false);
            }
        }
    }

    public void ChangeVolume(float volume)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tv"))
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                onlineController.photonView.RPC("ChangeVolume", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, volume);
            }
        }
    }

    public void PauseVideo(bool value)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tv"))
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                onlineController.photonView.RPC("PauseVideo", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, value);
            }
        }
    }

    public void RewindVideo(float sec)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tv"))
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                onlineController.photonView.RPC("RewindVideo", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID, sec);
            }
        }
    }

    public void OffAudio()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.CompareTag("tv"))
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                onlineController.photonView.RPC("OffAudio", RpcTarget.AllBufferedViaServer, prop.GetPhotonView().ViewID);
            }
        }
    }
}
