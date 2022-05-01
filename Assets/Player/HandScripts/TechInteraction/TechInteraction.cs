using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechInteraction : MonoBehaviour
{
    public GameObject body, prop, useButton, exitButton;

    private void Update()
    {
        if (body.GetComponent<Player>().mode == "pc")
        {
            if (Input.GetKeyDown(KeyCode.G)) Interact();
        }
        else if (body.GetComponent<Player>().mode == "phone")
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
            {
                if (hit.collider.tag == "tech")
                {
                    useButton.SetActive(true);
                }
                else useButton.SetActive(false);
            }
            else useButton.SetActive(false);
        }
    }

    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tech")
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null && prop.transform.parent.tag == "tech") prop = prop.transform.parent.gameObject;

                if (prop.GetComponent<Technology>() != null && prop.GetComponent<Technology>().needGenerator == false)
                {
                    prop.GetComponent<Technology>().Activate(!prop.GetComponent<Technology>().isWorking);

                    if (prop.GetComponent<Technology>().isWorking && prop.name.Contains("camera")) exitButton.SetActive(!exitButton.active);
                }

                else if (prop.GetComponent<GeneratorAdditionals>() != null) prop.GetComponent<GeneratorAdditionals>().Action();
            }
        }
    }
}
