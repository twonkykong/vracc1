using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeight : MonoBehaviour
{
    public GameObject body, prop, toolgun;
    public Text currentMass;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop = hit.collider.gameObject;
                while (prop.GetComponent<Rigidbody>() == null) prop = prop.transform.parent.gameObject;

                currentMass.gameObject.SetActive(true);
                currentMass.text = "mass: " + System.Math.Round(prop.GetComponent<Rigidbody>().mass, 1);
                if (prop.GetComponent<Rigidbody>().useGravity == false) currentMass.text = "gravity off";

                if (body.GetComponent<Player>().mode == "pc")
                {
                    if (Input.GetMouseButtonDown(0)) IncreaseWeight();
                    if (Input.GetMouseButtonDown(1)) ReduceWeight();
                    if (Input.GetMouseButtonDown(2)) SwitchGravity();
                }
            }
            else currentMass.gameObject.SetActive(false);
        }
        else currentMass.gameObject.SetActive(false);
    }

    public void IncreaseWeight()
    {
        prop.GetComponent<Rigidbody>().mass *= 1.1f;
        toolgun.GetComponent<ToolgunControll>().Click();
    }

    public void ReduceWeight()
    {
        prop.GetComponent<Rigidbody>().mass /= 1.1f;
        toolgun.GetComponent<ToolgunControll>().Click();
    }

    public void SwitchGravity()
    {
        if (prop.GetComponent<Rigidbody>().useGravity == true) prop.GetComponent<Rigidbody>().useGravity = false;
        else prop.GetComponent<Rigidbody>().useGravity = true;
        toolgun.GetComponent<ToolgunControll>().Click();
    }
}
