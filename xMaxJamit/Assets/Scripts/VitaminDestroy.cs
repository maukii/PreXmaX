using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminDestroy : MonoBehaviour {

    public float timeToDie;

    void Update ()
    {
        timeToDie -= 1 * Time.deltaTime;

        if (timeToDie <= 0)
        {
            Destroy(gameObject);
        }
    }
}
