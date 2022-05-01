 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerMP : MonoBehaviourPunCallbacks
{
    public string[] components;
    public GameObject[] hide;

    public GameObject mesh, toolgun, eventObj, body, nicknameObj;
    public Text info, players;
    public bool adminRights;

    private void Start()
    {
        if (!this.photonView.IsMine)
        {
            foreach (string component in components)
            {
                (GetComponent(component) as MonoBehaviour).enabled = false;
            }

            foreach (GameObject obj in hide) obj.SetActive(false);
            GetComponentInChildren<PlayerPhoneMove>().enabled = false;
        }

        else
        {
            mesh.layer = 10;
            foreach (Transform child in toolgun.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 10;
            }

            GameObject.Find("gameManager").GetComponent<gameManager>().eventObj = eventObj;

            this.photonView.RPC("ChangeColor", RpcTarget.AllBufferedViaServer, mesh.GetPhotonView().ViewID, System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[0]), System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[1]), System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[2]));
            this.photonView.RPC("ChangeNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName, nicknameObj.GetPhotonView().ViewID);

            info.text = PhotonNetwork.NickName + "\n" + PhotonNetwork.CurrentRoom.Name + " (" + PhotonNetwork.CurrentRoom.PlayerCount + " players)\n";
            nicknameObj.SetActive(false);
        }
    }

    [PunRPC]
    public void ChangeColor(int propViewID, float r, float g, float b)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        Material mat = new Material(prop.GetComponent<SkinnedMeshRenderer>().material);
        mat.color = new Color(r / 255, g / 255, b / 255);
        prop.GetComponent<SkinnedMeshRenderer>().material = mat;
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    public void ChangeNickName(string nickname, int textViewiD)
    {
        GameObject nicknameObj = PhotonView.Find(textViewiD).gameObject;
        nicknameObj.GetComponent<TextMeshPro>().text = nickname;
    }

    [PunRPC]
    public void PlaySound(int audiosrcViewID, string audioName, float delay = 0)
    {
        AudioSource audiosrc = PhotonView.Find(audiosrcViewID).GetComponent<AudioSource>();
        audiosrc.Stop();
        audiosrc.clip = (AudioClip)Resources.Load(audioName);
        audiosrc.pitch = 1 + Random.Range(-0.1f, 0.1f);
        audiosrc.PlayDelayed(delay);
    }

    [PunRPC]
    public void GetDamage(int ViewID, float damage)
    {
        if (PhotonView.Find(ViewID).IsMine) GetComponent<Player>().GetDamage(damage);
    }
}
