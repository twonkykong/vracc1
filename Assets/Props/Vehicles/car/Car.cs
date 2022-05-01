using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public WheelCollider[] wheels;
    public GameObject steeringWheel;
    public float speed, brake, steer;

    public float motorTorque, brakeTorque, steerAngle;

    public string mode = "pc";

    private void Update()
    {
        foreach (WheelCollider wheel in wheels)
        {
            if (wheel.gameObject.name == "r1")
            {
                if (mode == "pc") steerAngle = Input.GetAxis("Horizontal") * steer;

                wheel.steerAngle = steerAngle;
                wheel.gameObject.transform.localRotation = Quaternion.Slerp(wheel.gameObject.transform.localRotation, Quaternion.Euler(new Vector3(wheel.gameObject.transform.localEulerAngles.x, 90 + wheel.steerAngle, wheel.gameObject.transform.localEulerAngles.z)), 0.4f);
                steeringWheel.transform.localRotation = Quaternion.Slerp(steeringWheel.transform.localRotation, Quaternion.Euler(new Vector3(steeringWheel.transform.localEulerAngles.x, steeringWheel.transform.localEulerAngles.y, wheel.steerAngle)), 0.4f);
            }
            else if (wheel.gameObject.name == "l1")
            {
                if (mode == "pc") steerAngle = Input.GetAxis("Horizontal") * steer;
                wheel.steerAngle = steerAngle;
                wheel.gameObject.transform.localRotation = Quaternion.Slerp(wheel.gameObject.transform.localRotation, Quaternion.Euler(new Vector3(wheel.gameObject.transform.localEulerAngles.x, -90 + wheel.steerAngle, wheel.gameObject.transform.localEulerAngles.z)), 0.4f);
            }
            else
            {
                if (mode == "pc") motorTorque = Input.GetAxis("Vertical") * speed;

                wheel.motorTorque = motorTorque;
                if (mode == "pc") brakeTorque = System.Convert.ToSingle(Input.GetKey(KeyCode.Space)) * brake;
                wheel.brakeTorque = brakeTorque;
            }
            wheel.gameObject.transform.Rotate(0, 0, wheel.rpm / 60 * 360 * Time.deltaTime, Space.Self);
            
        }
    }
}
