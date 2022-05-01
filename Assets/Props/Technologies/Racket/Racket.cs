using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    public Rigidbody rb;
    public float force, maxSpeed = 10;
    public GameObject fire;

    private void Update()
    {
        rb.AddForce(transform.up * force, ForceMode.Impulse);
        if (rb.velocity.x > maxSpeed) rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.y > maxSpeed) rb.velocity = new Vector3(rb.velocity.x, maxSpeed, rb.velocity.z);
        if (rb.velocity.z > maxSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);

        if (rb.velocity.x < -maxSpeed) rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.y < -maxSpeed) rb.velocity = new Vector3(rb.velocity.x, -maxSpeed, rb.velocity.z);
        if (rb.velocity.z < -maxSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxSpeed);
    }

    private void OnEnable()
    {
        fire.SetActive(true);
    }

    private void OnDisable()
    {
        fire.SetActive(false);
    }
}
