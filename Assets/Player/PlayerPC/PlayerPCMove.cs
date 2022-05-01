using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPCMove : MonoBehaviour
{
    Rigidbody rb;
    public Animator anim;

    public float speed, jumpForce;
    public Transform jumpRayPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) anim.SetBool("walk", true);
        else anim.SetBool("walk", false);

        Vector3 pos = transform.right * (Input.GetAxis("Horizontal") * speed) + transform.forward * (Input.GetAxis("Vertical") * speed);
        Vector3 newPos = new Vector3(pos.x, rb.velocity.y, pos.z);
        rb.velocity = newPos;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(jumpRayPoint.position, -transform.up, 1f))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (!Physics.Raycast(jumpRayPoint.transform.position, -transform.up, 0.5f)) anim.SetBool("jump", true);
        else anim.SetBool("jump", false);

        rb.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("jump", false);
            anim.SetBool("crouch", true);
            speed = 2;
        }
        else
        {
            anim.SetBool("crouch", false);
            speed = 5;
        }
    }
}
