using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public Joint joint;
    public Joint[] startJoints;
    public List<Joint> connectedJoints;
    public bool holded, isKinematic;
    public AudioSource audio;

    public List<Vector3> bouncingPoints = new Vector3[1] { Vector3.zero }.ToList();

    private void Start()
    {
        GameObject g = new GameObject();
        g.name = name;
        if (GetComponentInChildren<Renderer>() != null)
        {
            Renderer[] components = GetComponentsInChildren<Renderer>();
            Vector3 center = Vector3.zero;

            foreach (Renderer renderer in components)
            {
                center += renderer.bounds.center;
            }
            center /= components.Length;
            g.transform.position = center;
        }
        else if (GetComponent<Renderer>() != null) g.transform.position = GetComponent<Renderer>().bounds.center;
        GetComponent<Rigidbody>().solverIterations = 60;
        name += " " + Time.time;

        audio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (GetComponent<Joint>() != null)
        {
            List<Joint> joints = GetComponents<Joint>().ToList();
            if (joint != null) joints.Remove(joint);
            if (startJoints != null) foreach (Joint joint in startJoints) joints.Remove(joint);
            LineRenderer[] liners = GetComponentsInChildren<LineRenderer>();

            for (int i = 0; i < joints.Count; i++)
            {
                if (joints[i].gameObject.name == "wire") return;
                if (joints[i].connectedBody == null)
                {
                    Destroy(joints[i]);
                    Destroy(liners[i].gameObject);
                }
                foreach (Transform obj in joints[i].connectedBody.gameObject.GetComponentInChildren<Transform>())
                {
                    if (obj.name == "weldingLine " + name + " " + i) liners[i].SetPositions(new Vector3[2] { liners[i].gameObject.transform.position, obj.position });
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if ((rb.velocity.x > 0 && rb.velocity.x < 1f) || (rb.velocity.x < 0 && rb.velocity.x > -1f) && (rb.velocity.y > 0 && rb.velocity.y < 1f) || (rb.velocity.y < 0 && rb.velocity.y > -1f) && (rb.velocity.z > 0 && rb.velocity.z < 1f) || (rb.velocity.z < 0 && rb.velocity.z > -1f)) return;
        audio.Stop();
        audio.pitch = 1 + Random.Range(-0.1f, 0.1f);
        audio.Play();
    }
}
