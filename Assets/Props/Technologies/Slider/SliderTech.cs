using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderTech : MonoBehaviour
{
    public GameObject point1, point2, obj, body;
    public bool movingToPoint1;
    public float timer;
    public float speed = 0.2f;

    private void FixedUpdate()
    {
        point2.transform.localPosition = new Vector3(-(body.transform.localScale.x - 0.5f), -0.1322267f);
        point1.transform.localPosition = new Vector3(body.transform.localScale.x - 0.5f, -0.1322267f);
        obj.transform.localPosition = new Vector3(Mathf.Clamp(obj.transform.localPosition.x, point2.transform.localPosition.x, point1.transform.localPosition.x), obj.transform.localPosition.y);
        if (timer != 0)
        {
            timer += 0.1f;
            if (timer >= 10)
            {
                timer = 0;
                movingToPoint1 = !movingToPoint1;
            }
        }
        else
        {
            if (movingToPoint1)
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, point1.transform.position, speed);
            }
            else
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, point2.transform.position, speed);
            }

            if ((obj.transform.position == point1.transform.position && movingToPoint1) || (obj.transform.position == point2.transform.position && !movingToPoint1)) timer = 0.1f;
        }
    }
}
