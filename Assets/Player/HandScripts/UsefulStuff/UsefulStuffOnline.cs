using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UsefulStuffOnline : MonoBehaviourPunCallbacks
{
    public GameObject body, chair, sitButton, vehicle, notRidingInput, ridingInput;
    public bool riding;

    public float brake, motor, steer;

    public OnlineController onlineController;

    IEnumerator update()
    {
        while (true)
        {
            if (chair == null) Sit(false);

            body.transform.position = chair.transform.position;
            body.transform.rotation = chair.transform.rotation;

            vehicle.GetComponent<Car>().steerAngle = steer;
            vehicle.GetComponent<Car>().motorTorque = motor;
            vehicle.GetComponent<Car>().brakeTorque = brake;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Sit(bool value)
    {
        if (value)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
            {
                if (hit.collider.tag == "chair")
                {
                    chair = hit.collider.gameObject;
                    while (chair.transform.parent != null && chair.transform.parent.tag == "chair") chair = chair.transform.parent.gameObject;
                    
                    sitButton.SetActive(value);
                }
            }
        }

        notRidingInput.SetActive(!value);

        if (chair.GetComponent<VehicleChair>().vehicle != null)
        {
            if (chair.GetComponent<VehicleChair>().componentName == "Car") vehicle = chair.GetComponent<VehicleChair>().vehicle;
            ridingInput.SetActive(value);

            vehicle.GetComponent<Car>().mode = body.GetComponent<Player>().mode;
        }
        
        chair.GetComponent<VehicleChair>().Press();

        riding = value;
        body.GetComponent<Animator>().SetBool("sit", value);
        body.GetComponent<PlayerPhoneMove>().enabled = !value;
        body.GetComponent<Rigidbody>().isKinematic = value;
        body.GetComponent<Collider>().enabled = !value;
        onlineController.photonView.RPC("Sit", RpcTarget.AllBufferedViaServer, chair.GetPhotonView().ViewID, value);
        StartCoroutine(update());

        if (!value)
        {
            chair = null;
            StopCoroutine(update());
        }
    }

    public void ChangeSteer(float value)
    {
        steer = vehicle.GetComponent<Car>().steer * value;
    }

    public void ChangeMotor(float value)
    {
        motor = vehicle.GetComponent<Car>().speed * value;
    }

    public void Brake(float value)
    {
        brake = vehicle.GetComponent<Car>().brake * value;
    }
}
