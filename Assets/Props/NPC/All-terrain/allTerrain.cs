using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allTerrain : MonoBehaviour
{
    public Vector3 pos;
    public float speed = 1;
    public Transform rayPoint;
    Quaternion rot1, lookAt;

    private void Start()
    {
        GeneratePos();
    }

    private void FixedUpdate()
    {
        if (transform.rotation != lookAt)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt, 0.9f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x, transform.position.y, pos.z), speed);
        }

        if (transform.position == new Vector3(pos.x, transform.position.y, pos.z)) GeneratePos();
        if (Physics.Raycast(rayPoint.transform.position, transform.forward, 1f) || !Physics.Raycast(transform.position + transform.forward + transform.up * 0.2f, -transform.up, 2f)) GeneratePos();
    
    }

    public void GeneratePos()
    {
        pos = new Vector3(transform.position.x + Random.Range(-15, 16), transform.position.y, transform.position.z + Random.Range(-15, 16));
        rot1 = transform.rotation;
        transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
        lookAt = transform.rotation;
        transform.rotation = rot1;
    }
}
