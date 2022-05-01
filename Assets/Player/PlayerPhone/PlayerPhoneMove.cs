using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using WaterBuoyancy;
using Photon.Pun.Demo.Procedural;

public class PlayerPhoneMove : MonoBehaviourPun
{
    Rigidbody rb;
    public Animator anim, weapons, hands;

    public GameObject joystick, leg1, leg2, head, waterVignette, water;
    public Camera[] cameras;
    public float speed, jumpForce;
    public Transform jumpRayPoint, weaponsHolder;
    Vector3 weaponsPos;

    bool crouching, online;
    public AudioSource audio;
    public AudioClip jump, land, walk;
    bool grounded, jumping;
    public float walkSpeedMultiplier = 1;

    Quaternion leg1StartRot, leg1PrevRot1, leg1PrevRot2, leg2StartRot, leg2PrevRot1, leg2PrevRot2;

    PlayerMP playerMp;
    RectTransform rect;

    int layermask = 1 << 4;

    private void Start()
    {
        layermask = ~layermask;
        rb = GetComponent<Rigidbody>();
        rect = joystick.GetComponent<RectTransform>();
        leg1StartRot = leg1.transform.rotation;
        leg2StartRot = leg2.transform.rotation;

        playerMp = GetComponentInParent<PlayerMP>();
    }

    void FixedUpdate()
    {
        leg1PrevRot1 = leg1.transform.rotation;
        leg2PrevRot1 = leg2.transform.rotation;
        leg1.transform.rotation = Quaternion.Euler(leg1StartRot.eulerAngles.x, leg1StartRot.eulerAngles.y + transform.rotation.eulerAngles.y, leg1StartRot.eulerAngles.z);
        leg2.transform.rotation = Quaternion.Euler(leg2StartRot.eulerAngles.x, leg2StartRot.eulerAngles.y + transform.rotation.eulerAngles.y, leg2StartRot.eulerAngles.z);

        if (joystick.transform.localPosition != Vector3.zero)
        {
            int modifier = 1;
            if (joystick.transform.localPosition.y / rect.sizeDelta.y < 0) modifier = -1;
            
            leg1.transform.Rotate(new Vector3(0, joystick.transform.localPosition.x / rect.sizeDelta.x * 60 * modifier, 0), Space.World);
            
            leg2.transform.Rotate(new Vector3(0, joystick.transform.localPosition.x / rect.sizeDelta.x * 60 * modifier, 0), Space.World);
            leg1PrevRot2 = leg1.transform.rotation;
            leg2PrevRot2 = leg2.transform.rotation;

            leg1.transform.rotation = Quaternion.Slerp(leg1PrevRot1, leg1PrevRot2, 0.2f);
            leg2.transform.rotation = Quaternion.Slerp(leg2PrevRot1, leg2PrevRot2, 0.2f);

            anim.SetBool("walk", true);
            weapons.SetBool("walk", true);
            if (Vector3.Distance(joystick.transform.localPosition, Vector3.zero) / rect.sizeDelta.x < 0.7f) anim.SetBool("slowWalk", true);
            else anim.SetBool("slowWalk", false);

            weaponsPos = new Vector3(0, 0, -Vector3.Distance(joystick.transform.localPosition, Vector3.zero) / rect.sizeDelta.x * 0.2f);
            if (!audio.isPlaying && !anim.GetBool("jump") && !anim.GetBool("swim"))
            {
                
                    playerMp.photonView.RPC("PlaySound", RpcTarget.AllViaServer, audio.gameObject.GetPhotonView().ViewID, "walk", 0.1525f);
                
                /*else 
                {
                    audio.clip = walk;
                    audio.pitch = 1 + Random.Range(-0.2f, 0.1f);
                    audio.PlayDelayed(0.1525f);
                }*/
            }
        }
        else
        {
            anim.SetBool("walk", false);
            weapons.SetBool("walk", false);
            anim.SetBool("slowWalk", false);

            weaponsPos = Vector3.zero;
        }


        anim.SetFloat("walkSpeed", Vector3.Distance(joystick.transform.localPosition, Vector3.zero) / rect.sizeDelta.x * walkSpeedMultiplier);
        weapons.SetFloat("walkSpeed", Vector3.Distance(joystick.transform.localPosition, Vector3.zero) / rect.sizeDelta.x * walkSpeedMultiplier);

        if (joystick.transform.localPosition.y / rect.sizeDelta.y < 0) anim.SetFloat("walkSpeed", -Mathf.Abs(anim.GetFloat("walkSpeed")));

        Vector3 pos = transform.right * (joystick.transform.localPosition.x / rect.sizeDelta.x * speed) + transform.forward * (joystick.transform.localPosition.y / rect.sizeDelta.x * speed);
        Vector3 newPos = new Vector3(pos.x, rb.velocity.y, pos.z);
        rb.velocity = newPos;

        if (!Physics.Raycast(jumpRayPoint.position, -transform.up, 0.5f, layermask))
        {
            if (!crouching) anim.SetBool("jump", true);
            weaponsPos = new Vector3(-0.05f, 0.15f, -Vector3.Distance(joystick.transform.localPosition, Vector3.zero) / rect.sizeDelta.x * 0.2f - 0.05f);
        }
        else
        {
            anim.SetBool("jump", false);
            if (!grounded)
            {
                
                    playerMp.photonView.RPC("PlaySound", RpcTarget.AllViaServer, audio.gameObject.GetPhotonView().ViewID, "land", 0f);
                
                /*else
                {
                    audio.Stop();
                    audio.clip = land;
                    audio.pitch = 1 + Random.Range(-0.1f, 0.1f);
                    audio.Play();
                }*/
            }
        }

        grounded = Physics.Raycast(jumpRayPoint.position, -transform.up, 0.5f, layermask);
        weaponsHolder.localPosition = Vector3.Slerp(weaponsHolder.localPosition, weaponsPos, 0.25f);

        hands.SetBool("walk", anim.GetBool("walk"));
        hands.SetBool("slowWalk", anim.GetBool("slowWalk"));
        hands.SetBool("jump", anim.GetBool("jump"));
        hands.SetBool("crouch", anim.GetBool("crouch"));
        hands.SetFloat("walkSpeed", anim.GetFloat("walkSpeed"));

        //jumping
        if (jumping)
        {
            if (water != null)
            {
                rb.AddForce(-Physics.gravity * 1.3f, ForceMode.Acceleration);
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(jumpRayPoint.position, -transform.up, out hit, 0.2f, layermask))
                {
                    
                        playerMp.photonView.RPC("PlaySound", RpcTarget.AllViaServer, audio.gameObject.GetPhotonView().ViewID, "jump", 0f);
                    
                    /*else
                    {
                        audio.Stop();
                        audio.clip = jump;
                        audio.pitch = 1 + Random.Range(-0.1f, 0.1f);
                        audio.Play();
                    }*/
                    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                    
                }
            }
        }
    }
    public void Jump()
    {
        jumping = !jumping;
    }

    public void Crouch()
    {
        crouching = !crouching;
        anim.SetBool("crouch", crouching);

        if (crouching)
        {
            speed = 4;
            anim.SetBool("jump", false);
        }
        else
        {
            speed = 5;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Bounds bounds = other.bounds;
        if (!ColliderUtils.IsPointInsideCollider(GetComponent<Collider>().ClosestPoint(bounds.ClosestPoint(transform.position)), other, ref bounds)) return;

        if (other.CompareTag("Water Volume"))
        {
            water = other.gameObject;
            rb.drag = 1; 
            anim.SetBool("swim", true);
            walkSpeedMultiplier = 0.6f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Bounds bounds = other.bounds;
        if (!ColliderUtils.IsPointInsideCollider(GetComponent<Collider>().ClosestPoint(bounds.ClosestPoint(transform.position)), other, ref bounds)) return;

        if (other.CompareTag("Water Volume"))
        {
            rb.AddForce(-Physics.gravity * 0.2f, ForceMode.Acceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (water != null && other.gameObject == water)
        {
            water = null;
            rb.drag = 0;
            anim.SetBool("swim", false);
            walkSpeedMultiplier = 1;
        }
    }
}
