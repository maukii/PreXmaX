using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminDestroy : MonoBehaviour {

    [SerializeField] float timeToDie;
    public bool timerActive;
    public bool hasThrown;

    [SerializeField] GameObject particle;
    [SerializeField] GameObject cam;

    [SerializeField] float power;
    [SerializeField] float radius;
    [SerializeField] float upforce;

    [SerializeField] int godValue;

    void Start()
    {
        timerActive = true;
        if(cam == null)
        {
            cam = GameObject.FindWithTag("MainCamera");
        }
    }

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
            Instantiate(particle, transform.position, Quaternion.identity);
            Explosion();
            cam.GetComponent<VitaminShake>().shakeDuration = 0.5f;
            cam.GetComponent<VitaminShake>().shakeAmount = 1f;
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
            {
                rb.AddExplosionForce(power, explosionPos, radius, upforce, ForceMode.Impulse);
            }
        }
    }
}