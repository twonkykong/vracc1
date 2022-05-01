using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaterBuoyancy;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Camera>() != null && other.GetComponent<Camera>().isActiveAndEnabled)
        {
            other.GetComponent<CamEffects>().waterVignette.SetActive(true);
            other.GetComponent<Camera>().fieldOfView *= 0.8f;
        }

        else if (other.gameObject.layer == 11 && (other.GetComponentInParent<FloatingObject>() != null || other.GetComponent<FloatingObject>() != null))
        {
            if (other.GetComponent<FloatingObject>() != null) other.GetComponent<FloatingObject>().water = GetComponent<WaterVolume>();
            else other.GetComponentInParent<FloatingObject>().water = GetComponent<WaterVolume>();

            if (other.GetComponent<FloatingObject>() != null && other.GetComponent<FloatingObject>().voxels != null)
            {
                other.GetComponent<FloatingObject>().voxels = other.GetComponent<FloatingObject>().CutIntoVoxels();
            }
            
            if (other.GetComponentInParent<FloatingObject>() != null && other.GetComponentInParent<FloatingObject>().voxels == null)
            {
                other.GetComponentInParent<FloatingObject>().voxels = other.GetComponentInParent<FloatingObject>().CutIntoVoxels();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Camera>() != null && other.GetComponent<Camera>().isActiveAndEnabled)
        {
            other.GetComponent<CamEffects>().waterVignette.SetActive(false);
            other.GetComponent<Camera>().fieldOfView /= 0.8f;
        }

        else if (other.gameObject.layer == 11 && (other.GetComponentInParent<FloatingObject>() != null || other.GetComponent<FloatingObject>() != null))
        {
            if (other.GetComponent<FloatingObject>() != null) other.GetComponent<FloatingObject>().water = null;
            else other.GetComponentInParent<FloatingObject>().water = null;
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + (-Physics.gravity / 9.81f * (other.transform.localScale.x + other.transform.localScale.y + other.transform.localScale.z)), Physics.gravity, out hit, other.transform.localScale.x + other.transform.localScale.y + other.transform.localScale.z);
            point = hit.point;

            Vector3 actionPoint = other.transform.position + other.transform.TransformDirection(point);
            Debug.Log(actionPoint);
            float forceFactor = 1f - ((actionPoint.y - 0) / 1);

            if (forceFactor > 0f)
            {
                Vector3 uplift = -Physics.gravity * (forceFactor - other.GetComponent<Rigidbody>().velocity.y * 0.05f);
                other.GetComponent<Rigidbody>().AddForceAtPosition(uplift, actionPoint);
            }
        }
    }*/
}
