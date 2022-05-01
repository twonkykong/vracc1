using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineChat : MonoBehaviour
{
    public InputField input;
    public Text text;
    public Animation anim;
    public GameObject player;

    bool expanded;

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
                    text.text += "\n(" + DateTime.Now.ToShortTimeString() + ") SERVER - Teleported to (" + x + ", " + y + ", " + z + ").";
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
                    if (obj.layer == 11)
                    {
                        Destroy(obj);
                        props += 1;
                    }
                }
                if (props > 0) text.text += "\n(" + DateTime.Now.ToShortTimeString() + ") SERVER - Cleared all props (" + props + ").";
            }

            else
            {
                text.text += "\n<color=#D92E2E>Unknown command. Type /help for the list of available commands.</color>";
            }
        }
        else text.text += "\n(" + DateTime.Now.ToShortTimeString() + ")" + input.text;
        input.text = "";
    }

    public void ExpandMenu()
    {
        if (!expanded) anim.Play("openChat");
        else anim.Play("closeChat");

        expanded = !expanded;
    }
}
