using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welding : MonoBehaviour
{
    public GameObject body, cam, prop1, prop2, toolgun, hands, menu, usefulThings;
    public bool welding, isMenu;
    public Material weldMat, ropeMat;
    public string jointType = "fixed";
    Vector3 difference;

    private void Update()
    {
        if (body.GetComponent<Player>().mode == "pc")
        {
            if (!welding)
            {
                if (Input.GetMouseButtonDown(1)) OpenWeldMenu(true);
                if (Input.GetMouseButtonUp(1)) OpenWeldMenu(false);
            }

            if (!isMenu)
            {
                if (!welding) if (Input.GetMouseButtonDown(0)) StartWelding();
                    else if (Input.GetMouseButtonDown(0)) EndWelding();
            }
        }
    }

    public void ChangeJoint(string type)
    {
        jointType = type;
    }

    public void OpenWeldMenu(bool value)
    {
        isMenu = value;
        menu.SetActive(value);

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
        toolgun.GetComponent<ToolgunControll>().canChange = !value;
        
    }

    public void StartWelding()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop1 = hit.collider.gameObject;
                while (prop1.GetComponent<Rigidbody>() == null) prop1 = prop1.transform.parent.gameObject;

                if (jointType == "remove")
                {
                    if (prop1.GetComponent<Joint>() != null)
                    {
                        foreach (Joint joint in prop1.GetComponents<Joint>())
                        {
                            if (prop1.GetComponent<Prop>().joint == null || joint != prop1.GetComponent<Prop>().joint)
                            {
                                Destroy(prop1.GetComponent<Joint>());
                                Destroy(prop1.GetComponentInChildren<LineRenderer>().gameObject);

                                break;
                            }
                        }
                    }
                    else
                    {
                        if (prop1.GetComponent<Prop>().connectedJoints.Count > 0)
                        {
                            prop1.GetComponent<Prop>().connectedJoints.RemoveAt(0);
                            Destroy(prop1.GetComponent<Prop>().connectedJoints[0]);
                        }
                    }
                }
                else
                {
                    welding = true;

                    usefulThings.SetActive(false);
                    toolgun.GetComponent<ToolgunControll>().canChange = false;
                    difference = hit.point - prop1.transform.position;
                }

                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void EndWelding()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                prop2 = hit.collider.gameObject;
                while (prop2.GetComponent<Rigidbody>() == null) prop2 = prop2.transform.parent.gameObject;

                bool welded = false;
                foreach (Joint joint in prop2.GetComponents<FixedJoint>())
                {
                    if (joint.connectedBody == prop1.GetComponent<Rigidbody>()) welded = true;
                }

                if (!welded)
                {
                    Joint joint = null;
                    if (jointType == "fixed")
                    {
                        joint = prop1.AddComponent<FixedJoint>();
                    }
                    else if (jointType == "spring")
                    {
                        joint = prop1.AddComponent<SpringJoint>();
                    }
                    else if (jointType == "hinge")
                    {
                        joint = prop1.AddComponent<HingeJoint>();
                    }

                    if (joint != null)
                    {
                        joint.connectedBody = prop2.GetComponent<Rigidbody>();
                        joint.connectedMassScale = prop1.GetComponent<Rigidbody>().mass + prop2.GetComponent<Rigidbody>().mass;

                        prop2.GetComponent<Prop>().connectedJoints.Add(joint);
                    }

                    GameObject g = new GameObject();
                    g.name = "new line";
                    g.transform.position = difference + prop1.transform.position;
                    g.transform.parent = prop1.transform;
                    g.AddComponent<LineRenderer>().startWidth = 0.2f;
                    g.GetComponent<Renderer>().material = weldMat;

                    int count = prop1.GetComponents<Joint>().Length;

                    GameObject g1 = new GameObject();
                    g1.name = "weldingLine " + prop1.name + " " + (count - 1);
                    g1.transform.position = hit.point;
                    g1.transform.parent = prop2.transform;
                }
            }
        }

        welding = false;

        usefulThings.SetActive(true);
        toolgun.GetComponent<ToolgunControll>().canChange = true;

        toolgun.GetComponent<ToolgunControll>().Click();
    }
}
