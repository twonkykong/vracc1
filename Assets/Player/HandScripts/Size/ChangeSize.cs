using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSize : MonoBehaviour
{
    public GameObject body, prop, toolgun, hands, cords;
    public bool x, y, z;
    public Toggle xToggle, yToggle, zToggle;

    private void Update()
    {
        if (body.GetComponent<Player>().mode == "pc")
        {
            if (Input.GetKeyDown(KeyCode.X)) x = true;
            if (Input.GetKeyUp(KeyCode.X)) x = false;

            if (Input.GetKeyDown(KeyCode.Y)) y = true;
            if (Input.GetKeyUp(KeyCode.Y)) y = false;

            if (Input.GetKeyDown(KeyCode.Z)) z = true;
            if (Input.GetKeyUp(KeyCode.Z)) z = false;

            if (Input.GetMouseButtonDown(0)) IncreaseSize();
            if (Input.GetMouseButtonDown(1)) ReduceSize();
        }
        else if (body.GetComponent<Player>().mode == "phone")
        {
            x = xToggle.isOn;
            y = yToggle.isOn;
            z = zToggle.isOn;
        }
        

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop = hit.collider.gameObject;
                cords.SetActive(true);
                cords.transform.rotation = prop.transform.rotation;
            }
            else cords.SetActive(false);
        }
        else cords.SetActive(false);
    }

    public void IncreaseSize()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop = hit.collider.gameObject;

                if (!x && !y && !z) prop.transform.localScale = prop.transform.localScale * 1.1f;
                else
                {
                    float xSize = prop.transform.localScale.x;
                    float ySize = prop.transform.localScale.y;
                    float zSize = prop.transform.localScale.z;

                    if (x == true) xSize *= 1.1f;
                    if (y == true) ySize *= 1.1f;
                    if (z == true) zSize *= 1.1f;

                    prop.transform.localScale = new Vector3(xSize, ySize, zSize);
                }

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void ReduceSize()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                if (!x && !y && !z) prop.transform.localScale = prop.transform.localScale / 1.1f;
                else
                {
                    float xSize = prop.transform.localScale.x;
                    float ySize = prop.transform.localScale.y;
                    float zSize = prop.transform.localScale.z;

                    if (x == true) xSize /= 1.1f;
                    if (y == true) ySize /= 1.1f;
                    if (z == true) zSize /= 1.1f;

                    prop.transform.localScale = new Vector3(xSize, ySize, zSize);
                }

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }
}
