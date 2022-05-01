using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Video;

public class YouTubeTVOnline : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        this.photonView.RPC("SyncVideo", RpcTarget.All, GetComponentInChildren<VideoPlayer>().time);
    }

    [PunRPC]
    public void SyncVideo(float time)
    {
        GetComponentInChildren<VideoPlayer>().time = time;
    }
}
