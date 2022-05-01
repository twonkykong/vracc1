using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public GameObject body, prop1, prop2, toolgun, usefulThings;
    public bool wiring, generatorFirst;

    private void Update()
    {
        if (body.GetComponent<Player>().mode == "pc")
        {
            if (!wiring)
            {
                if (Input.GetMouseButtonDown(0)) StartWiring();
                if (Input.GetMouseButtonDown(1)) RemoveWire();
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) EndWiring();
            }
        }
    }

    public void StartWiring()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tech")
            {
                prop1 = hit.collider.gameObject;
                while (prop1.transform.parent != null && prop1.transform.parent.tag == "tech") prop1 = prop1.transform.parent.gameObject;
                if (prop1.GetComponent<Technology>() != null && !prop1.GetComponent<Technology>().canWire) return;
                wiring = true;
                toolgun.GetComponent<ToolgunControll>().canChange = false;
                usefulThings.SetActive(false);


                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void EndWiring()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tech")
            {
                prop2 = hit.collider.gameObject;
                while (prop2.transform.parent != null && prop2.transform.parent.tag == "tech") prop2 = prop2.transform.parent.gameObject;
                if (prop2.GetComponent<Technology>() != null && !prop2.GetComponent<Technology>().canWire) return;

                if (prop1.GetComponent<Technology>() != null || prop2.GetComponent<Technology>() != null)
                {
                    if (prop1.GetComponent<Technology>() == null || prop1.GetComponent<Technology>().generator != null)
                    {
                        prop1.GetComponent<PowerGenerator>().EnableConnection(prop2);
                    }
                    else
                    {
                        prop2.GetComponent<PowerGenerator>().EnableConnection(prop1);
                    }
                }

                wiring = false;
                toolgun.GetComponent<ToolgunControll>().canChange = true;
                usefulThings.SetActive(true);

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void RemoveWire()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tech")
            {
                prop1 = hit.collider.gameObject;
                while (prop1.transform.parent != null && prop1.transform.parent.tag == "tech") prop1 = prop1.transform.parent.gameObject;

                if (prop1.GetComponent<Technology>() != null && prop1.GetComponent<Technology>().generator != null)
                {
                    prop1.GetComponent<Technology>().generator.GetComponent<PowerGenerator>().connectedTechs.Remove(prop1);
                    prop1.GetComponent<Technology>().generator = null;
                    prop1.GetComponent<Technology>().Activate(false);
                }

                else if (prop1.GetComponent<PowerGenerator>() != null && prop1.GetComponent<PowerGenerator>().connectedTechs.Count != 0)
                {
                    prop1.GetComponent<PowerGenerator>().connectedTechs[prop1.GetComponent<PowerGenerator>().connectedTechs.Count - 1].GetComponent<Technology>().generator = null;
                    prop1.GetComponent<PowerGenerator>().connectedTechs[prop1.GetComponent<PowerGenerator>().connectedTechs.Count - 1].GetComponent<Technology>().Activate(false);

                    prop1.GetComponent<PowerGenerator>().connectedTechs.RemoveAt(prop1.GetComponent<PowerGenerator>().connectedTechs.Count - 1);
                }
            }
        }
    }
}
