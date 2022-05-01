using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

public class Pistol : MonoBehaviourPun
{
    public GameObject bulletPrefab, magPrefab;
    public Animator anim;
    public AudioSource audiosrc;
    public Text ammoText;
    public PlayerMP playerMp;
    public int fullMagAmmo = 15;
    public int ammo = 15;
    public int totalAmmo;
    bool canShoot = true;

    private void Start()
    {
        ammoText.text = ammo + "/" + fullMagAmmo + " - " + totalAmmo;
    }

    IEnumerator ShotTimer()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("shot", false);
    }

    IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("reload", false);
        if (totalAmmo > fullMagAmmo) ammo = fullMagAmmo;
        else ammo = totalAmmo;

        totalAmmo -= ammo;
        canShoot = true;
        ammoText.text = ammo + "/" + fullMagAmmo + " - " + totalAmmo;
    }

    public void Shoot()
    {
        if (!canShoot) return;
        if (ammo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, 200f))
            {
                if (hit.collider.gameObject.layer == 11) hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(transform.parent.forward * 4f, hit.point, ForceMode.Impulse);
                else if (hit.collider.CompareTag("Player")) hit.collider.GetComponent<PlayerMP>().photonView.RPC("GetDamage", RpcTarget.All, hit.collider.gameObject.GetPhotonView().ViewID);
            }
            ammo -= 1;
            playerMp.photonView.RPC("PlaySound", RpcTarget.AllViaServer, audiosrc.gameObject.GetPhotonView().ViewID, "pistolShot", 0f);
            anim.Play("idle");
            anim.SetBool("shot", true);
            StartCoroutine(ShotTimer());
            ammoText.text = ammo + "/" + fullMagAmmo + " - " + totalAmmo;
        }

        else if (totalAmmo > 0)
        {
            Reload();
        }

        else
        {
            playerMp.photonView.RPC("PlaySound", RpcTarget.AllViaServer, audiosrc.gameObject.GetPhotonView().ViewID, "emptyPistol", 0f);
        }
    }

    public void Reload()
    {
        if (!canShoot) return;
        if (totalAmmo > 0 && ammo < fullMagAmmo)
        {
            canShoot = false;
            audiosrc.Stop();
            playerMp.photonView.RPC("PlaySound", RpcTarget.AllViaServer, audiosrc.gameObject.GetPhotonView().ViewID, "pistolReload", 0f);
            anim.Play("idle");
            anim.SetBool("reload", true);
            StartCoroutine(ReloadTimer());
        }
        else if (totalAmmo < 0)
        {
            playerMp.photonView.RPC("PlaySound", RpcTarget.AllViaServer, audiosrc.gameObject.GetPhotonView().ViewID, "emptyPistol", 0f);
        }
    }
}
