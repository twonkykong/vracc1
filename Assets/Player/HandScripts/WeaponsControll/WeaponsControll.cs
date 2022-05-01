using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsControll : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject[] buttons;
    public int weapon;
    public Animation anim;
    bool opened;
    public TextMeshProUGUI weaponText;
    public Animator hands;
    public Animation weaponsAnim;
    public AudioSource audio;

    public void OpenMenu()
    {
        anim.Stop();
        if (opened) anim.Play("close");
        else anim.Play("open");

        opened = !opened;
    }

    public void ChangeWeapon(string params_)
    {
        int number = int.Parse(params_.Split('/')[0]);
        string name = params_.Split('/')[1];

        if ((name != "" && buttons[weapon].GetComponentInChildren<Text>().text == name) || (number != 0 && weapon == number))
        {
            anim.Stop();
            anim.Play("close");
            opened = !opened;

            return;
        }
        foreach (GameObject obj in weapons[weapon].GetComponent<Weapon>().objects) obj.SetActive(false);
        
        weapons[weapon].SetActive(false);

        if (number != 0)
        {
            weapon += number;

            if (weapon >= weapons.Length - 1) weapon = 0;
            else if (weapon < 0) weapon = weapons.Length - 1;

            weapons[weapon].SetActive(true);
            foreach (GameObject obj in weapons[weapon].GetComponent<Weapon>().objects) obj.SetActive(true);

            audio.Stop();
            audio.clip = (AudioClip)Resources.Load("changeWeapon" + Random.Range(0, 2));
            audio.pitch = 1 + Random.Range(-0.1f, 0.1f);
            audio.Play();

            weaponsAnim.Stop();
            weaponsAnim.Play();
        }

        else if (name != "")
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetComponentInChildren<Text>().text == name)
                {
                    weapons[i].SetActive(true);
                    weapon = i;
                    foreach (GameObject obj in weapons[weapon].GetComponent<Weapon>().objects) obj.SetActive(true);

                    audio.Stop();
                    audio.clip = (AudioClip)Resources.Load("changeWeapon" + Random.Range(0, 2));
                    audio.pitch = 1 + Random.Range(-0.1f, 0.1f);
                    audio.Play();

                    weaponsAnim.Stop();
                    weaponsAnim.Play();
                    break;
                }
            }
        }

        weaponText.text = buttons[weapon].GetComponentInChildren<Text>().text;
        hands.SetInteger("weapon", weapon);
        anim.Stop();
        anim.Play("close");
        opened = !opened;
    }
}
