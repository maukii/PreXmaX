using UnityEngine;
using System.Collections;

public class VitaminRandomizer : MonoBehaviour {

    public GameObject[] Vitamin;
    public Vector3 center;
    public Vector3 size;

    public GameObject particle;

    public float waitTime = 2f;

	void Start ()
    {
        StartCoroutine(AutoSpawn());
	}

    IEnumerator AutoSpawn()
    {
        while (true)
        { 
            yield return new WaitForSeconds(waitTime);
            SpawnVitamins();
        }
    }
	
	void SpawnVitamins ()
    {
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        Instantiate(Vitamin[Random.Range(0, 3)], pos, Quaternion.identity);
        Instantiate(particle, pos, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}