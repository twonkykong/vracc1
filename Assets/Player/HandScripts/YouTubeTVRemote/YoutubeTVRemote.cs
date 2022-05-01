using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class YoutubeTVRemote : MonoBehaviour
{
    public InputField text;
    public GameObject prop, cam, body, toolgun, menu;

    public void OpenMenu(bool value)
    {
        if (value)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
            {
                if (hit.collider.tag == "tv")
                {
                    menu.SetActive(value);
                    if (body.GetComponent<Player>().mode == "pc")
                    {
                        cam.GetComponent<PlayerPCCameraLook>().enabled = !value;
                        if (value) Cursor.lockState = CursorLockMode.None;
                        else Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = value;
                    }
                    else
                    {
                        cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
                    }
                }
            }
        }
        else
        {
            menu.SetActive(value);
            if (body.GetComponent<Player>().mode == "pc")
            {
                cam.GetComponent<PlayerPCCameraLook>().enabled = !value;
                if (value) Cursor.lockState = CursorLockMode.None;
                else Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = value;
            }
            else
            {
                cam.GetComponent<PlayerPhoneCameraLook>().enabled = !value;
            }
        }
    }

    public void ChangeVideo()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tv")
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
                youtubePlayer.GetComponentInChildren<VideoPlayer>().Stop();
                youtubePlayer.url = text.text; 
                youtubePlayer.Play(); 

                prop.GetComponentInChildren<Animation>().Stop();
                prop.GetComponentInChildren<Animation>().Play("play");
                OpenMenu(false);
            }
        }
    }

    public void ChangeVolume(float volume)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tv")
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
                youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute = false;
                youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).volume += volume;

                prop.GetComponentInChildren<Animation>().Stop();
                prop.GetComponentInChildren<Animation>().Play("changeVolume");

                prop.GetComponentInChildren<TextMeshPro>().text = youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).volume * 100 + "%";
            }
        }
    }

    public void PauseVideo(bool value)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tv")
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
                if (value) youtubePlayer.GetComponentInChildren<VideoPlayer>().Pause();
                else youtubePlayer.GetComponentInChildren<VideoPlayer>().Play();

                prop.GetComponentInChildren<Animation>().Stop();
                if (value) prop.GetComponentInChildren<Animation>().Play("pause");
                else prop.GetComponentInChildren<Animation>().Play("play");
            }
        }
    }

    public void RewindVideo(float sec)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tv")
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
                youtubePlayer.GetComponentInChildren<VideoPlayer>().time += sec;

                prop.GetComponentInChildren<Animation>().Stop();
                if (sec > 0) prop.GetComponentInChildren<Animation>().Play("rewind+");
                else prop.GetComponentInChildren<Animation>().Play("rewind-");
            }
        }
    }

    public void OffAudio()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            if (hit.collider.tag == "tv")
            {
                prop = hit.collider.gameObject;
                while (prop.transform.parent != null) prop = prop.transform.parent.gameObject;
                var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
                youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute = !youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute;

                prop.GetComponentInChildren<Animation>().Stop();
                if (youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute == true) prop.GetComponentInChildren<Animation>().Play("off volume");
                else prop.GetComponentInChildren<Animation>().Play("on volume");
            }
        }
    }
}
