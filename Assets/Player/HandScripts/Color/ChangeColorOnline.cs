using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Photon.Pun;

public class ChangeColorOnline : MonoBehaviourPunCallbacks
{
    public Slider r, g, b, a;
    public bool isMenu;
    public GameObject body, menuObj, cam, toolgun;
    public Text rText, gText, bText, aText;
    public Image colorPreview;
    public OnlineController onlineController;
    int layermask = 1 << 4;

    private void Start()
    {
        layermask = ~layermask;
    }

    IEnumerator update()
    {
        while (true)
        {
            rText.text = "" + r.value;
            gText.text = "" + g.value;
            bText.text = "" + b.value;
            aText.text = "" + a.value;

            colorPreview.color = new Color(r.value / 255, g.value / 255, b.value / 255, a.value / 255);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ChangeColorClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                Debug.Log(hit.collider.gameObject.name);
                onlineController.photonView.RPC("ChangeColor", RpcTarget.AllBufferedViaServer, hit.collider.gameObject.GetPhotonView().ViewID, r.value, g.value, b.value, a.value);

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void CopyColor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                r.value = hit.collider.gameObject.GetComponent<Renderer>().material.color.r * 255;
                g.value = hit.collider.gameObject.GetComponent<Renderer>().material.color.g * 255;
                b.value = hit.collider.gameObject.GetComponent<Renderer>().material.color.b * 255;
                a.value = hit.collider.gameObject.GetComponent<Renderer>().material.color.a * 255;
            }
        }
    }

    public void OpenMenu(bool value)
    {
        if (value) StartCoroutine(update());
        else StopCoroutine(update());

        isMenu = value;
        menuObj.SetActive(value);

        cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
    }
}
