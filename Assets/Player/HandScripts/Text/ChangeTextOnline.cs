using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChangeTextOnline : MonoBehaviourPun
{
    public InputField text;
    public GameObject prop, cam, body, toolgun, menu;
    public TMP_FontAsset font;
    public TextMeshProUGUI textPreview;
    public OnlineController onlineController;

    IEnumerator update()
    {
        while (true)
        {
            textPreview.text = text.text;
            yield return new WaitForEndOfFrame();
        }
    }
        
    public void OpenMenu(bool value)
    {
        if (value) StartCoroutine(update());
        else StopCoroutine(update());
        menu.SetActive(value);
        cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
    }

    public void ChangeTextClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.collider.tag == "text")
            {
                onlineController.photonView.RPC("ChangeText", RpcTarget.AllBufferedViaServer, hit.collider.gameObject.GetPhotonView().ViewID, text.text, font.name);
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
