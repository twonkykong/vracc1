using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string mode = "pc";
    public GameObject cam, gameUi, menu, mesh, ragdollPrefab, ragdoll, buttons, deadVignette;
    public float hp = 100, timer, money;

    public Toggle hideToggle;

    PlayerPhoneMove move;

    private void Start()
    {
        move = GetComponent<PlayerPhoneMove>();
        if (GameObject.FindObjectOfType<PhotonView>() == null)
        {
            Material mat = new Material(mesh.GetComponent<SkinnedMeshRenderer>().material);
            mat.color = new Color(System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[0]) / 255, System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[1]) / 255, System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[2]) / 255);
            mesh.GetComponent<SkinnedMeshRenderer>().material = mat;
        }
    }

    private IEnumerator corutineTimer()
    {
        yield return new WaitForSeconds(5);

        hp = 100;
        Destroy(ragdoll);
        cam.GetComponent<PlayerPhoneCameraLook>().xAngle = 0;
        cam.GetComponent<PlayerPhoneCameraLook>().yAngle = 0;
        GetComponent<PlayerPhoneMove>().joystick.transform.localPosition = Vector3.zero;
        transform.position = new Vector3(0, 5, 0);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<PlayerPhoneMove>().enabled = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if (child.name == "Armature")
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
        buttons.SetActive(!hideToggle.isOn);
        deadVignette.SetActive(false);
        GetComponent<Animator>().SetBool("crouch", false);

        if (move.water != null)
        {
            move.water = null;
            GetComponent<Rigidbody>().drag = 0;
            move.anim.SetBool("swim", false);
            move.walkSpeedMultiplier = 1;
            foreach (Camera cam in GetComponentInChildren<PlayerPhoneCameraLook>().cameras)
            {
                cam.fieldOfView /= 0.8f;
            }
        }
        move.waterVignette.SetActive(false);
    }


    public void Pause(bool value)
    {
        gameUi.SetActive(!value);
        menu.SetActive(value);
        if (mode == "pc")
        {
            GetComponent<PlayerPCMove>().enabled = !value;
            cam.GetComponent<PlayerPCCameraLook>().enabled = !value;
        }
        else if (mode == "phone")
        {
            GetComponent<PlayerPhoneMove>().enabled = !value;
            cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
        }
    }

    public void Leave()
    {
        Application.LoadLevel("Menu");
    }

    public void GetDamage(float damage)
    {
        hp -= Mathf.RoundToInt(damage);
        if (hp <= 0)
        {
            if (ragdoll != null) return;
            ragdoll = PhotonNetwork.Instantiate(ragdollPrefab.name, transform.position, transform.rotation);
            ragdoll.GetComponentInChildren<PlayerPhoneRagdollLook>().xAngle = cam.GetComponent<PlayerPhoneCameraLook>().xAngle;
            ragdoll.GetComponentInChildren<PlayerPhoneRagdollLook>().yAngle = cam.GetComponent<PlayerPhoneCameraLook>().yAngle;
            ragdoll.GetComponentInChildren<CamEffects>().waterVignette = move.waterVignette;
            foreach (Collider col in ragdoll.GetComponentsInChildren<Collider>()) col.gameObject.layer = 13;

            if (cam.GetComponent<PlayerPhoneCameraLook>().cam == 2) ragdoll.GetComponentInChildren<PlayerPhoneRagdollLook>().xAngle += 180;

            GetComponent<PlayerMP>().photonView.RPC("ChangeColor", RpcTarget.AllViaServer, ragdoll.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.GetPhotonView().ViewID, System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[0]), System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[1]), System.Convert.ToSingle(PlayerPrefs.GetString("bodyColor").Split('/')[2]));

            foreach (Transform child in ragdoll.GetComponentInChildren<Transform>())
            {
                if (child.name == "Armature")
                {
                    child.GetComponentInChildren<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 2;
                    break;
                }
            }

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<PlayerPhoneMove>().enabled = false;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            foreach (Transform child in GetComponentInChildren<Transform>())
            {
                if (child.name == "Armature")
                {
                    child.gameObject.SetActive(false);
                    break;
                }
            }
            buttons.SetActive(false);
            deadVignette.SetActive(true);
            StartCoroutine(corutineTimer());
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Rigidbody>() != null)
        {
            if (collision.collider.gameObject.layer != 11) return;
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            if (rb.velocity.magnitude >= 1)
            {
                Debug.Log(rb.velocity.magnitude);
                GetDamage(rb.velocity.magnitude * 30);
            }
        }
    }*/

    public void HideButtons()
    {
        buttons.SetActive(!hideToggle.isOn);
    }
}
