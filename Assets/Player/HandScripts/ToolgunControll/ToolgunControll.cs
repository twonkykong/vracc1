using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ToolgunControll : MonoBehaviourPun
{
    public GameObject[] weapons, hints;
    public GameObject hands, body, tshirt;
    public int weaponNumber;
    public Text weaponsText;
    public Animator anim;
    public Slider slider;
    public AudioSource audio;

    public float timer;
    public bool canChange;

    IEnumerator clickTimer()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("click1", false);
        anim.SetBool("click2", false);

        hands.GetComponent<Animator>().SetBool("click", false);
    }

    public void Hold(bool value)
    {
        canChange = !value;
        GetComponent<Animator>().SetBool("hold", value);
        hands.GetComponent<Animator>().SetBool("hold", value);
    }

    public void Click()
    {
        GetComponent<Animator>().Play("idle");
        GetComponent<Animator>().SetBool("click" + Random.Range(1, 3), true);
        StartCoroutine(clickTimer());
        hands.GetComponent<Animator>().SetBool("click", true);

        body.GetComponentInParent<PlayerMP>().photonView.RPC("PlaySound", RpcTarget.AllViaServer, audio.gameObject.GetPhotonView().ViewID, "toolgunClick", 0f);
    }

    public void ChangeWeapon(int number)
    {
        if (canChange)
        {
            weapons[weaponNumber].SetActive(false);

            weaponNumber = System.Convert.ToInt32(slider.value);

            if (weaponNumber > weapons.Length - 1) weaponNumber = 0;
            else if (weaponNumber < 0) weaponNumber = weapons.Length - 1;

            weapons[weaponNumber].SetActive(true);

            weaponsText.text = weapons[0].name;
            if (weaponNumber == 0) weaponsText.text += " <";

            for (int i = 1; i < weapons.Length; i++)
            {
                weaponsText.text += "\n" + weapons[i].name;
                if (weaponNumber == i) weaponsText.text += " <";
            }

            foreach (GameObject obj in hints) if (obj != null) obj.SetActive(false);
            if (hints[weaponNumber] != null) hints[weaponNumber].SetActive(true);
            
        }
    }
}
