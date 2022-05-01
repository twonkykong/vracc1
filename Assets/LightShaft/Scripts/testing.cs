using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    public YoutubePlayer yp;

    void Start()
    {
        yp.Play(yp.youtubeUrl);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
