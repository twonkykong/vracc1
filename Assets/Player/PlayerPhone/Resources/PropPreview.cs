/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaterBuoyancy;

public class PropPreview : MonoBehaviour
{
    Renderer renderer;
    public Transform cam;
    int layermask = 1 << 4;

    private void Start()
    {
        layermask = ~layermask;
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        transform.position = cam.position + cam.forward * 6;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 6f))
        {
            transform.position = hit.point;
        }

        Vector3 upperPoint = renderer.bounds.ClosestPoint(cam.up * (transform.localScale.x + transform.localScale.y + transform.localScale.z));
        Vector3 frontPoint = renderer.bounds.ClosestPoint(-cam.forward * (transform.localScale.x + transform.localScale.y + transform.localScale.z));
        float upperDistance = Vector3.Distance(renderer.bounds.center, upperPoint);
        float frontDistance = Vector3.Distance(renderer.bounds.center, frontPoint);

        if (Physics.BoxCast(renderer.bounds.center + (Vector3.up * upperDistance * 2), renderer.bounds.size, -Vector3.up, out hit, transform.rotation))
        {
            if (renderer.bounds.Contains(hit.point))
            {
                Vector3 point = renderer.bounds.ClosestPoint(-Vector3.up * (transform.localScale.x + transform.localScale.y + transform.localScale.z));
                float distance = Vector3.Distance(hit.point, point);

                transform.position += Vector3.up * distance;
            }
        }

        if (Physics.BoxCast(renderer.bounds.center - (Vector3.forward * frontDistance * 2), renderer.bounds.size, Vector3.forward, out hit, transform.rotation))
        {
            if (renderer.bounds.Contains(hit.point))
            {
                Vector3 point = renderer.bounds.ClosestPoint(Vector3.forward * (transform.localScale.x + transform.localScale.y + transform.localScale.z));
                float distance = Vector3.Distance(hit.point, point);

                transform.position -= Vector3.forward * distance;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, renderer.bounds.size);
    }
}
*/