using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhoneCameraLook : MonoBehaviour
{
    Vector3 FirstPoint;
    Vector3 SecondPoint;
    public float xAngle;
	public float yAngle;
    float xAngleTemp;
    float yAngleTemp;
	int touchid;

	public float sensitivity = 1;
	float slerpTime = 1;

	public GameObject head, body, usefulStuff;
	public GameObject[] cams, camHolders;
	public int cam;

	public Camera[] cameras;

	bool zoom;

    void Start()
    {
        //copied
        xAngle = 0;
        yAngle = 0;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, xAngle, 0);
        transform.rotation = Quaternion.Euler(-yAngle, transform.eulerAngles.y, 0);

		sensitivity = PlayerPrefs.GetInt("sensitivity") / 10;

		foreach (Camera cam in cameras) cam.fieldOfView = PlayerPrefs.GetInt("fov");
	}

    void FixedUpdate()
    {
		//head rotation
		if (Input.touchCount > 0)
		{
			if (Input.touchCount > 1)
			{
				foreach (Touch touch in Input.touches) if (touch.position.x > Screen.width / 2) touchid = touch.fingerId;
			}
			else touchid = 0;

			if (Input.GetTouch(touchid).position.x > Screen.width / 2)
			{
				if (Input.GetTouch(touchid).phase == TouchPhase.Began)
				{
					FirstPoint = Input.GetTouch(touchid).position;
					xAngleTemp = xAngle;
					yAngleTemp = yAngle;
				}
				if (Input.GetTouch(touchid).phase == TouchPhase.Moved)
				{
					SecondPoint = Input.GetTouch(touchid).position;
					xAngle = xAngleTemp + (SecondPoint.x - FirstPoint.x) * 180 * sensitivity / Screen.width;
					yAngle = yAngleTemp + (SecondPoint.y - FirstPoint.y) * 90 * sensitivity / Screen.height;

					if (yAngle >= 89.9f) yAngle = 89.9f;
					if (yAngle <= -89.9f) yAngle = -89.9f;
					if (xAngle >= 360 || xAngle <= -360) xAngle = 0;
				}
			}
		}

		if (usefulStuff.GetComponent<UsefulStuffOnline>().riding == false)
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-yAngle, 0, 0.0f), slerpTime);
			body.transform.rotation = Quaternion.Slerp(body.transform.rotation, Quaternion.Euler(0.0f, xAngle, 0.0f), slerpTime);
			head.transform.localRotation = transform.localRotation;
		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-yAngle, xAngle, 0.0f), slerpTime);
			head.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}

		foreach (GameObject cam in cams) cam.transform.localPosition = Vector3.zero;

		int layermask1 = 1 << 4;
		int layermask2 = 1 << 13;
		int layermask = layermask1 | layermask2;
		layermask = ~layermask;


		RaycastHit hit;
		if (Physics.Linecast(transform.position, cams[0].transform.position, out hit, layermask))
		{
			cams[0].transform.localPosition = new Vector3(0, 0, Vector3.Distance(camHolders[0].transform.position, hit.point));
		}

		RaycastHit hit1;
		if (Physics.Linecast(transform.position, cams[1].transform.position, out hit1, layermask))
		{
			cams[1].transform.localPosition = new Vector3(0, 0, -Vector3.Distance(camHolders[1].transform.position, hit1.point));
		}
	}

	public void Zoom()
	{
		if (!zoom)
		{
			foreach (Camera cam in cameras) cam.fieldOfView /=3;
			slerpTime = 0.2f;
		}

		else
		{
			foreach (Camera cam in cameras) cam.fieldOfView *= 3;
			slerpTime = 1;
		}

		zoom = !zoom;
	}

	public void ChangeView()
	{
		cam += 1;
		if (cam > cams.Length - 1) cam = 0;
		else if (cam < 0) cam = cams.Length - 1;

		foreach (GameObject cam in cams) cam.SetActive(false);

		cams[cam].SetActive(true);
	}
}
