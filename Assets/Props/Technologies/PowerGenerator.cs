using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerGenerator : MonoBehaviour
{
    public List<GameObject> connectedTechs;
    public int maxTechs;
    public bool isWorking;

    private void Start()
    {
        isWorking = false;
    }

    private void Update()
    {
        if (connectedTechs.Count != 0)
        {
            foreach (GameObject tech in connectedTechs) tech.GetComponent<Technology>().Activate(isWorking);
        }
    }

    public void EnableConnection(GameObject tech)
    {
        if (connectedTechs.Count >= maxTechs) return;

        connectedTechs.Add(tech);
        tech.GetComponent<Technology>().generator = gameObject;
    }

    private void OnEnable()
    {
        isWorking = true;

        if (connectedTechs.Count != 0)
        {
            foreach (GameObject tech in connectedTechs) tech.GetComponent<Technology>().Activate(isWorking);
        }
    }

    private void OnDisable()
    {
        isWorking = false;

        if (connectedTechs.Count != 0)
        {
            foreach (GameObject tech in connectedTechs) tech.GetComponent<Technology>().Activate(isWorking);
        }
    }
}
