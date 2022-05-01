using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;

public class SpawnerOnline : MonoBehaviourPunCallbacks
{
    public GameObject propPreview, prop, toolgun, cam, body, hands, propMenu, duplicateProp;
    public bool isMenu;
    int layermask = 1 << 4;

    private void Start()
    {
        layermask = ~layermask;
    }

    public void OpenMenu(bool value)
    {
        isMenu = value;
        propMenu.SetActive(value);
        toolgun.GetComponent<ToolgunControll>().canChange = !value;
        
        cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
    }

    public void Create()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            GameObject g = PhotonNetwork.Instantiate(prop.name, hit.point, Quaternion.Euler(0, body.transform.eulerAngles.y, 0));
            Bounds bounds;
            if (g.GetComponent<Renderer>() != null)
            {
                bounds = g.GetComponent<Renderer>().bounds;
            }
            else
            {
                bounds = g.GetComponentInChildren<Renderer>().bounds;
                foreach (Renderer renderer in g.GetComponentsInChildren<Renderer>())
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }

            Debug.Log(g.transform.position);
            Vector3 upperPoint = bounds.center + Vector3.up * (g.transform.localScale.x + g.transform.localScale.y + g.transform.localScale.z);
            Vector3 closestUp = bounds.ClosestPoint(upperPoint);

            Vector3 lowerPoint = bounds.center - Vector3.up * (g.transform.localScale.x + g.transform.localScale.y + g.transform.localScale.z);
            Vector3 closestDown = bounds.ClosestPoint(lowerPoint);

            float rayDistance = Vector3.Distance(closestUp, closestDown);

            if (Physics.Raycast(new Vector3(g.transform.position.x, closestUp.y, g.transform.position.z), -Vector3.up, rayDistance))
            {
                Vector3 point = bounds.center - Vector3.up * (g.transform.localScale.x + g.transform.localScale.y + g.transform.localScale.z);
                Vector3 closest = bounds.ClosestPoint(point);
                float distance = Vector3.Distance(closest, hit.point);
                Debug.Log(point + " / " + closest + " / " + distance);
                g.transform.position += Vector3.up * distance;
            }

            if (prop == duplicateProp) g.SetActive(true);
            toolgun.GetComponent<ToolgunControll>().Click();
        }
    }
    
    public void Delete()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                GameObject obj = hit.collider.gameObject;
                while (obj.GetComponent<Rigidbody>() == null) obj = obj.transform.parent.gameObject;
                if (obj.GetComponent<Prop>().holded) return;

                obj.GetPhotonView().TransferOwnership(this.photonView.Owner);
                PhotonNetwork.Destroy(obj);
                toolgun.GetComponent<ToolgunControll>().Click();
            }
        }
    }

    public void Duplicate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, layermask))
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
