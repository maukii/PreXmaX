using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminDestroy : MonoBehaviour {

    [SerializeField] float timeToDie;
    public bool timerActive;
    public bool hasThrown;

    [SerializeField] float power;
    [SerializeField] float radius;
    [SerializeField] float upforce;

    [SerializeField] int godValue;

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
            Explosion();
            Destroy(gameObject);
        }
    }

    void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, upforce, ForceMode.Impulse);
        }
    }
}