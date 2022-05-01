using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Slider r, g, b, a;
    public bool isMenu;
    public GameObject body, menuObj, cam, toolgun;
    public Text rText, gText, bText, aText;
    public Image colorPreview;

    private void Update()
    {
        if (!isMenu)
        {
            if (body.GetComponent<Player>().mode == "pc")
            {
                if (Input.GetMouseButtonDown(0)) ChangeColorClick();
                if (Input.GetMouseButtonDown(2)) CopyColor();
            }
        }
        else
        {
            rText.text = "" + r.value;
            gText.text = "" + g.value;
            bText.text = "" + b.value;
            aText.text = "" + a.value;

            colorPreview.color = new Color(r.value / 255, g.value / 255, b.value / 255, a.value / 255);
        }

        if (body.GetComponent<Player>().mode == "pc")
        {
            if (Input.GetMouseButtonDown(1)) OpenMenu(true);
            if (Input.GetMouseButtonUp(1)) OpenMenu(false);
        }
    }

    public void ChangeColorClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                GameObject prop = hit.collider.gameObject;

                if (prop.tag == "text")
                {
                    prop.GetComponent<TextMeshPro>().color = new Color(r.value / 255, g.value / 255, b.value / 255, a.value / 255);
                }
                else
                {
                    Material mat = new Material(prop.GetComponent<Renderer>().material);
                    mat.color = new Color(r.value / 255, g.value / 255, b.value / 255, a.value / 255);
                    prop.GetComponent<Renderer>().material = mat;
                }

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void CopyColor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
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
        isMenu = value;
        menuObj.SetActive(value);

        if (body.GetComponent<Player>().mode == "pc")
        {
            cam.GetComponent<PlayerPCCameraLook>().enabled = !value;
            if (value) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = value;
        }
        else if (body.GetComponent<Player>().mode == "phone")
        {
            cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
        }
    }
}
