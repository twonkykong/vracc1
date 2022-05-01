using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMP : MonoBehaviour
{
    void Update()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("playerNickname")) obj.transform.LookAt(transform.position);
    }
}
