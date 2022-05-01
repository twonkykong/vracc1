using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefulStuff : MonoBehaviour
{
    public GameObject body, chair, sitButton, vehicle, notRidingInput, ridingInput;
    public bool riding;

    public float brake, motor, steer;

    private void Update()
    {
        if (!riding)
        {
            if (body.GetComponent<Player>().mode == "pc")
            {
                if (Input.GetKeyDown(KeyCode.F)) Sit(true);
            }
            else if (body.GetComponent<Player>().mode == "phone")
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
                {
                    if (hit.collider.tag == "chair")
                    {
                        sitButton.SetActive(true);
                    }
                    else sitButton.SetActive(false);
                }
                else sitButton.SetActive(false);
            }
        }

        else
        {
            if (chair == null) Sit(false);

            body.transform.position = chair.transform.position;
            body.transform.rotation = chair.transform.rotation;

            if (body.GetComponent<Player>().mode == "pc")
            {
                if (Input.GetKeyDown(KeyCode.F)) Sit(false);
            }

            vehicle.GetComponent<Car>().steerAngle = steer;
            vehicle.GetComponent<Car>().motorTorque = motor;
            vehicle.GetComponent<Car>().brakeTorque = brake;

        }
    }

    public void Sit(bool value)
    {
        if (value)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
            {
                if (hit.collider.tag == "chair")
                {
                    chair = hit.collider.gameObject;
                    while (chair.transform.parent != null && chair.transform.parent.tag == "chair") chair = chair.transform.parent.gameObject;
                    
                    if (body.GetComponent<Player>().mode == "phone")
                    {
                        sitButton.SetActive(value);
                    }
                }
            }
        }

        if (body.GetComponent<Player>().mode == "phone")
        {
            notRidingInput.SetActive(!value);

            if (chair.GetComponent<VehicleChair>().vehicle != null)
            {
                if (chair.GetComponent<VehicleChair>().componentName == "Car") vehicle = chair.GetComponent<VehicleChair>().vehicle;
                ridingInput.SetActive(value);

                vehicle.GetComponent<Car>().mode = body.GetComponent<Player>().mode;
            }
        }
        
        chair.GetComponent<VehicleChair>().Press();

        riding = value;
        body.GetComponent<Animator>().SetBool("sit", value);
        if (body.GetComponent<Player>().mode == "pc") body.GetComponent<PlayerPCMove>().enabled = !value;
        else if (body.GetComponent<Player>().mode == "phone") body.GetComponent<PlayerPhoneMove>().enabled = !value;
        body.GetComponent<Rigidbody>().isKinematic = value;
        body.GetComponent<Collider>().enabled = !value;


        if (!value)
        {
            chair = null;
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
