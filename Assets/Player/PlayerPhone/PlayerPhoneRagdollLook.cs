using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhoneRagdollLook : MonoBehaviour
{
	Vector3 FirstPoint;
	Vector3 SecondPoint;
	public float xAngle;
	public float yAngle;
	float xAngleTemp;
	float yAngleTemp;
	int touchid;

	public float sensitivity = 1;
	public GameObject cam, camHolder, waterVignette;
	bool zoom;

	void Start()
	{
		//copied
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, xAngle, 0);
		transform.rotation = Quaternion.Euler(-yAngle, transform.eulerAngles.y, 0);

		sensitivity = PlayerPrefs.GetInt("sensitivity") / 10;

		cam.GetComponent<Camera>().fieldOfView = PlayerPrefs.GetInt("fov");
	}

	void Update()
	{
		transform.parent.transform.rotation = Quaternion.Euler(Vector3.zero);
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

		transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-yAngle, xAngle, 0.0f), 1);

		cam.transform.localPosition = Vector3.zero;

		int layermask = 1 << 13;
		layermask = ~layermask;
		RaycastHit hit;
		if (Physics.Linecast(transform.position, cam.transform.position, out hit, layermask))
		{
			cam.transform.localPosition = new Vector3(0, 0, Vector3.Distance(camHolder.transform.position, hit.point));
		}
	}
}
