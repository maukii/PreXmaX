using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminDestroy : MonoBehaviour {

    [SerializeField] float stunDur = 3f;
    [SerializeField] float timeToDie;
    public bool timerActive;
    public bool hasThrown;

    [SerializeField] GameObject redParticle;
    [SerializeField] GameObject blueParticle;
    [SerializeField] GameObject greenParticle;
    [SerializeField] GameObject hitMard;
    GameObject cam;

    [SerializeField] float power;
    [SerializeField] float radius;
    [SerializeField] float upforce;

    public enum PillColor { Red, Green, Blue };
    public PillColor CurrentColor;

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
            switch(CurrentColor)
            {

                case PillColor.Blue:
                    Instantiate(blueParticle, transform.position, Quaternion.identity);
                    break;


                case PillColor.Red:
                    Instantiate(redParticle, transform.position, Quaternion.identity);
                    break;

                case PillColor.Green:
                    Instantiate(greenParticle, transform.position, Quaternion.identity);
                    break;

                default:
                    print("pill no color");
                    break;
            }

            Explosion();
            AudioManager.instance.PlaySoundEffect("oof", 1f);
            Instantiate(hitMard, transform.position, Quaternion.identity);
            cam.GetComponent<VitaminShake>().shakeDuration = 0.5f;
            cam.GetComponent<VitaminShake>().shakeAmount = 1f;
            Destroy(gameObject);

            if(other.gameObject.GetComponent<PlayerController>() != null)
            {
                // if only plays destroy pills
            }
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

            if (hit.gameObject.GetComponent<PlayerController>() != null)
            {
                hit.gameObject.GetComponent<PlayerController>().Stun(2f);
            }
        }
    }
}