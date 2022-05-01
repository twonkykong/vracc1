using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorAdditionals : MonoBehaviour
{
    public Animation anim;
    float timer;
    public bool workOne;

    public void Action()
    {
        GetComponent<PowerGenerator>().isWorking = !GetComponent<PowerGenerator>().isWorking;
        if (anim != null)
        {
            anim.Stop();
            if (anim.GetClipCount() == 1) anim.Play();
            else
            {
                if (GetComponent<PowerGenerator>().isWorking) anim.Play("on");
                else anim.Play("off");
            }

            if (workOne && GetComponent<PowerGenerator>().isWorking) timer = 0.1f;
        }
    }

    private void FixedUpdate()
    {
        if (timer != 0)
        {
            timer += 0.1f;
            if (timer >= 1)
            {
                timer = 0;
                GetComponent<PowerGenerator>().isWorking = false;
            }
        }
    }
}
