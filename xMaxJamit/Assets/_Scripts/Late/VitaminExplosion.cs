using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminExplosion : MonoBehaviour {

    public GameObject Player;
    public float power;
    public float radius;
    public float upforce;

	 void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Explosion();
        }
    }

    void Explosion()
    {
        Vector3 explosionPos = Player.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, upforce, ForceMode.Impulse);
        }
    }
}
