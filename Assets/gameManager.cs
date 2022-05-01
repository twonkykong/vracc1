using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;
using System;

public class gameManager : MonoBehaviourPunCallbacks
{
    public GameObject player, eventObj;
    public Text chatText;

    private void Start()
    {
        if (PhotonNetwork.OfflineMode) PhotonNetwork.Instantiate(player.name, new Vector3(0, 5, 0), Quaternion.identity);
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.OfflineMode) PhotonNetwork.Instantiate(player.name, new Vector3(0, 5, 0), Quaternion.identity);
    }

    private void Update()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("playerNickname"))
        {
            obj.GetComponent<TextMeshPro>().text = obj.transform.parent.gameObject.GetPhotonView().Owner.NickName;
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        eventObj.GetComponentInChildren<Text>().text = newPlayer.NickName + " joined room";
        eventObj.GetComponent<Animation>().Stop();
        eventObj.GetComponent<Animation>().Play();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        eventObj.GetComponentInChildren<Text>().text = otherPlayer.NickName + " left room";
        eventObj.GetComponent<Animation>().Stop();
        eventObj.GetComponent<Animation>().Play();
    }

    public override void OnLeftRoom()
    {
        Application.LoadLevel("Menu");
    }

    //CHAT
    [PunRPC]
    public new void SendMessage(string text)
    {
        chatText.text += "\n(" + DateTime.Now.ToShortTimeString() + ") " + text;
    }
}
