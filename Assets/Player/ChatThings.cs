using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ChatThings : MonoBehaviourPun
{
    public gameManager gameManager;
    public InputField input;
    public Text text;
    public Animation anim;
    public GameObject player;

    bool expanded;

    private void Start()
    {
        gameManager = GameObject.Find("gameManager").GetComponent<gameManager>();
        gameManager.chatText = text;
    }

    public void SendMessage()
    {
        if (input.text.Split(' ')[0].Contains("/"))
        {
            if (input.text == "/help")
            {
                text.text += "\n<color=#CFCFCF>/cords - see your current cords\n/tp 1 1 1 - teleport to given cords.\n/clear - delete all your props\n--for admins--\n/clear all - delete all objects\n/kick nickname - kick player by given name</color>";
            }

            else if (input.text.Split(' ')[0] == "/cords")
            {
                text.text += "\n<color=#CFCFCF>Your cords: " + System.Math.Round(player.transform.position.x, 2) + " " + Math.Round(player.transform.position.y, 2) + " " + Math.Round(player.transform.position.z, 2) + "</color>";
            }

            else if (input.text.Split(' ')[0] == "/tp")
            {
                float x, y, z;
                try
                {
                    x = float.Parse(input.text.Split(' ')[1]);
                    y = float.Parse(input.text.Split(' ')[2]);
                    z = float.Parse(input.text.Split(' ')[3]);

                    player.transform.position = new Vector3(x, y, z);
                    gameManager.photonView.RPC("SendMessage", RpcTarget.AllBufferedViaServer, "SERVER - Teleported " + PhotonNetwork.NickName + " to (" + x + ", " + y + ", " + z + ").");
                }
                catch
                {
                    text.text += "\n<color=#D92E2E>Invalid arguments. Try this: /tp 1 2.5 0</color>";
                }
            }

            else if (input.text == "/clear")
            {
                int props = 0;
                foreach (GameObject obj in FindObjectsOfType<GameObject>())
                {
                    if (obj.layer == 11 && obj.GetComponent<PhotonView>() != null && obj.GetPhotonView().IsMine)
                    {
                        PhotonNetwork.Destroy(obj);
                        props += 1;
                    }
                }
                if (props > 0) gameManager.photonView.RPC("SendMessage", RpcTarget.AllBufferedViaServer, "SERVER - Cleared all " + PhotonNetwork.NickName + "'s props (" + props + ").");
            }

            else if (input.text == "/clear all")
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    int props = 0;
                    foreach (GameObject obj in FindObjectsOfType<GameObject>())
                    {
                        if (obj.layer == 11 && obj.GetComponent<PhotonView>() != null)
                        {
                            props += 1;
                            obj.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
                            PhotonNetwork.Destroy(obj);
                        }
                    }

                    if (props > 0)
                    {
                        gameManager.photonView.RPC("SendMessage", RpcTarget.AllBufferedViaServer, "SERVER - Cleared all props (" + props + ").");
                    }
                }
                else
                {
                    text.text += "\n<color=#D92E2E>Access denied: only for admins.</color>";
                }
            }

            else if (input.text.Split(' ')[0] == "/clear")
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    bool foundPlayer = false;
                    foreach (var player in PhotonNetwork.PlayerList)
                    {
                        if (player.NickName == input.text.Replace("/clear ", ""))
                        {
                            foundPlayer = true;
                            int props = 0;
                            foreach (GameObject obj in FindObjectsOfType<GameObject>())
                            {
                                if (obj.layer == 11 && obj.GetComponent<PhotonView>() != null)
                                {
                                    if (obj.GetPhotonView().Owner == player)
                                    {
                                        props += 1;
                                        obj.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
                                        PhotonNetwork.Destroy(obj);
                                    }
                                    
                                }
                            }

                            if (props > 0)
                            {
                                gameManager.photonView.RPC("SendMessage", RpcTarget.AllBufferedViaServer, "SERVER - Cleared all " + player.NickName + "'s props (" + props + ").");
                            }
                        }
                    }

                    if (!foundPlayer)
                    {
                        text.text += "\n<color=#D92E2E>Couldn't find " + input.text.Replace("/clear ", "") + " in current room.</color>";
                    }
                }
                else
                {
                    text.text += "\n<color=#D92E2E>Access denied: only for admins.</color>";
                }
            }

            else if (input.text.Split(' ')[0] == "/kick") 
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    bool foundPlayer = false;
                    foreach (var player in PhotonNetwork.PlayerList)
                    {
                        if (player.NickName == input.text.Replace("/kick ", ""))
                        {
                            foundPlayer = true;
                            PhotonNetwork.CloseConnection(player);
                            gameManager.photonView.RPC("SendMessage", RpcTarget.AllBufferedViaServer, "SERVER - Kicked " + player.NickName + ".");
                        }
                    }

                    if (!foundPlayer)
                    {
                        text.text += "\n<color=#D92E2E>Couldn't find " + input.text.Replace("/kick ", "") + " in current room.</color>";
                    }
                }
                else
                {
                    text.text += "\n<color=#D92E2E>Access denied: only for admins.</color>";
                }
            }
            else
            {
                text.text += "\n<color=#D92E2E>Unknown command. Type /help for the list of available commands.</color>";
            }
        }
        else gameManager.photonView.RPC("SendMessage", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName + " : " +input.text);
        input.text = "";
    }

    public void ExpandMenu()
    {
        if (!expanded) anim.Play("openChat");
        else anim.Play("closeChat");

        expanded = !expanded;
    }
}
