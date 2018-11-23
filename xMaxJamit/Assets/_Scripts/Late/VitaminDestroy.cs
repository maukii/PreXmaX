using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminDestroy : MonoBehaviour {

    [SerializeField] float timeToDie;
    public bool timerActive;
    public bool hasThrown;

    void Update ()
    {
        if (timerActive)
        {
            timeToDie -= 1 * Time.deltaTime;

            if (timeToDie <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(hasThrown)
        {
            Destroy(gameObject);
        }
    }
}