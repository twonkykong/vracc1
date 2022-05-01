using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhoneVRMove : MonoBehaviour
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
        if (System.Math.Round(Input.GetAxis("LeftStickHorizontal"), 1) != 0 || System.Math.Round(-Input.GetAxis("LeftStickVertical"), 1) != 0) anim.SetBool("walk", true);
        else anim.SetBool("walk", false);

        Vector3 pos = transform.right * System.Convert.ToSingle(System.Math.Round(Input.GetAxis("LeftStickHorizontal"), 1) * speed) + transform.forward * System.Convert.ToSingle(System.Math.Round(-Input.GetAxis("LeftStickVertical"), 1) * speed);
        Vector3 newPos = new Vector3(pos.x, rb.velocity.y, pos.z);
        rb.velocity = newPos;

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (Physics.Raycast(jumpRayPoint.position, -transform.up, 1f))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (!Physics.Raycast(jumpRayPoint.transform.position, -transform.up, 0.5f)) anim.SetBool("jump", true);
        else anim.SetBool("jump", false);

        rb.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            anim.SetBool("jump", false);
            anim.SetBool("crouch", true);
            speed = 20;
        }
        else
        {
            anim.SetBool("crouch", false);
            speed = 50;
        }
    }
}
