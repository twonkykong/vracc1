using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class YouTubeTV : MonoBehaviour
{
    public TextMeshPro volumeText;
    public AudioSource audiosrc;
    public Transform progress;

    private void Update()
    {
        volumeText.text = Mathf.RoundToInt(audiosrc.volume * 100) + "%";
        if (GetComponentInChildren<VideoPlayer>().isPlaying)
        {
            progress.localScale = new Vector3(System.Convert.ToSingle(GetComponentInChildren<VideoPlayer>().time / GetComponentInChildren<VideoPlayer>().length * 5.3f), progress.localScale.y, progress.localScale.z);
        }
    }
}
