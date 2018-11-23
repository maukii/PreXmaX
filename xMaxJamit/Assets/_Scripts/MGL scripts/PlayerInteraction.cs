using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public List<GameObject> pills = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(!pills.Contains(other.gameObject) && other.GetComponent<VitaminDestroy>() != null)
        {
            pills.Add(other.gameObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < pills.Count; i++)
        {
            if(pills[i] == null)
            {
                pills.Remove(pills[i]);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(pills.Contains(other.gameObject))
        {
            pills.Remove(other.gameObject);
        }
    }

}
