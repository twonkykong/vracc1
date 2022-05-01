using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : MonoBehaviour
{
    public GameObject prop, toolgun, cam, body, hands, propMenu, duplicateProp, crossPrefab;
    public bool isMenu;

    Player player;

    private void Start()
    {
        player = body.GetComponent<Player>();
    }

    private void Update()
    {
        if (player.mode == "pc")
        {
            if (!isMenu)
            {
                if (Input.GetKeyDown(KeyCode.Q)) OpenMenu(true);
                if (Input.GetMouseButtonDown(0)) Create();
                if (Input.GetMouseButtonDown(1)) Delete();
            }

            else
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape)) OpenMenu(false);
            }
        }
    }

    public void OpenMenu(bool value)
    {
        isMenu = value;
        propMenu.SetActive(value);
        toolgun.GetComponent<ToolgunControll>().canChange = !value;
        if (player.mode == "pc")
        {
            cam.GetComponent<PlayerPCCameraLook>().enabled = !value;
            if (value) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = value;
        }
        else if (player.mode == "phone")
        {
            cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
            body.GetComponent<PlayerPhoneMove>().joystick.transform.localPosition = Vector3.zero;
        }

        
    }

    public void Create()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            GameObject g = Instantiate(prop, hit.point, Quaternion.Euler(0, body.transform.eulerAngles.y, 0));
            if (g.name.Contains("camera"))
            {
                g.transform.position = transform.position;
                g.transform.rotation = transform.rotation;
            }
            if (prop == duplicateProp) g.SetActive(true);
            toolgun.GetComponent<ToolgunControll>().Click();
        }
    }
    
    public void Delete()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                GameObject obj = hit.collider.gameObject;
                while (obj.GetComponent<Rigidbody>() == null) obj = obj.transform.parent.gameObject;

                Destroy(obj);
                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void Duplicate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                GameObject obj = hit.collider.gameObject;
                while (obj.transform.parent != null) obj = obj.transform.parent.gameObject;

                duplicateProp = Instantiate(obj, Vector3.zero, Quaternion.identity);
                duplicateProp.SetActive(false);
                if (duplicateProp.GetComponent<Technology>() != null)
                {
                    duplicateProp.GetComponent<Technology>().generator = null;
                    duplicateProp.GetComponent<Technology>().Activate(false);
                }
                else if (duplicateProp.GetComponent<PowerGenerator>() != null) duplicateProp.GetComponent<PowerGenerator>().connectedTechs = null;

                if (duplicateProp.GetComponent<Prop>().connectedJoints != null)
                {
                    foreach (Joint joint in duplicateProp.GetComponent<Prop>().connectedJoints) Destroy(joint);
                }

                if (duplicateProp.GetComponents<Joint>() != null)
                {
                    foreach (Joint joint in duplicateProp.GetComponents<Joint>())
                    {
                        if (duplicateProp.GetComponent<Prop>().joint != null)
                        {
                            if (joint == duplicateProp.GetComponent<Prop>().joint) continue;
                        }
                        Destroy(joint);
                    }
                    
                }
                prop = duplicateProp;
            }
        }
    }
}
