using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public InputField text;
    public GameObject prop, cam, body, toolgun, menu;
    public TMP_FontAsset font;
    public TextMeshProUGUI textPreview;

    void Update()
    {
        if (body.GetComponent<Player>().mode == "pc")
        {
            
        }

        textPreview.text = text.text;
    }

    public void OpenMenu(bool value)
    {
        menu.SetActive(value);
        if (body.GetComponent<Player>().mode == "pc")
        {
            cam.GetComponent<PlayerPCCameraLook>().enabled = !value;
            if (value) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = value;
        }
        else
        {
            cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
        }
    }

    public void ChangeTextClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "text")
            {
                prop = hit.collider.gameObject;
                prop.GetComponent<TextMeshPro>().text = text.text;
                prop.GetComponent<TextMeshPro>().font = font;
            }
        }
    }

    public void ChangeFont(TMP_FontAsset newFont)
    {
        font = newFont;
        textPreview.font = font;
    }
}
