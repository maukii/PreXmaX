using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GodHand : MonoBehaviour
{
    public int number = 1;
    public float power, radius, upforce;

    public float cPower, cRadius, cUpforce;

    public GameObject happyParticle, madParticle, endParticle;
    public Transform particlePos;

    Slider slider;

    public List<GameObject> players = new List<GameObject>();
    public GameObject[] pills = new GameObject[3];

    public VitaminDestroy.PillColor wantedPillColor;
    public Transform pillPosition;

    public GameObject[] winners = new GameObject[2];
    GameObject cam;
    public GameObject music;
    public AudioClip headache;

    private void Start()
    {
        if (cam == null)
        {
            cam = GameObject.FindWithTag("MainCamera");
        }

        slider = FindObjectOfType<Slider>();

        for (int i = 0; i < pills.Length; i++)
        {
            pills[i].gameObject.SetActive(false);
        }

        ChangePill();
    }

    public void ChangePill()
    {
        for (int i = 0; i < pills.Length; i++)
        {
            pills[i].gameObject.SetActive(false);
        }

        int wantedPill = UnityEngine.Random.Range(0, pills.Length);
        pills[wantedPill].SetActive(true);

        if(pills[0].activeSelf)
        {
            wantedPillColor = VitaminDestroy.PillColor.Red;
        }
        else if(pills[1].activeSelf)
        {
            wantedPillColor = VitaminDestroy.PillColor.Green;
        }
        else
        {
            wantedPillColor = VitaminDestroy.PillColor.Blue;
        }

        pills[wantedPill].transform.position = pillPosition.position;
    }

    public void GivePill(GameObject pill, int playerNumber, GameObject otherPlayer)
    {
        if(pill.GetComponent<VitaminDestroy>().CurrentColor == wantedPillColor)
        {
            GetHappy(playerNumber);
        }
        else
        {
            GetMad(otherPlayer, playerNumber);
        }
    }

    public void GetHappy(int playerNumber)
    {
        Instantiate(happyParticle, particlePos.position, Quaternion.identity);

        if(playerNumber == number)
        {
            if(playerNumber == 1)
            {
                slider.value += 1;
            }
            else
            {
                slider.value -= 1;
            }
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, cRadius);
        foreach (Collider hit in colliders)
        {

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(cPower, explosionPos, cRadius, cUpforce, ForceMode.Impulse);
            }

            if (hit.gameObject.GetComponent<PlayerController>() != null)
            {
                hit.gameObject.GetComponent<PlayerController>().Stun(2f);
            }
        }

        if (slider.value == 5 || slider.value == 15)
        {
            StartCoroutine(WaitForSong());
        }

        if (slider.value <= 0 || slider.value >= 20)
        {
            EndGame();
        }

        ChangePill();
    }

    public void GetMad(GameObject otherPlayer, int playerNumber)
    {
        if (number == playerNumber)
        {
            Instantiate(madParticle, particlePos.position, Quaternion.identity);

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

            if (playerNumber == 1)
            {
                slider.value -= 1;
            }
            else
            {
                slider.value += 1;
            }

            ChangePill();
        }
        else
        {
            Instantiate(madParticle, otherPlayer.transform.position, Quaternion.identity);

            Vector3 explosionPos = otherPlayer.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {

                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(power, new Vector3(explosionPos.x * UnityEngine.Random.Range(-0.5f, 0.5f),
                                                            explosionPos.y,
                                                            explosionPos.z * UnityEngine.Random.Range(-0.5f, 0.5f)),
                                            radius, upforce * 2, ForceMode.Impulse);
                }

                if (hit.gameObject.GetComponent<PlayerController>() != null)
                {
                    print(hit.name);
                    hit.gameObject.GetComponent<PlayerController>().Stun(2f);
                }
            }

            if (playerNumber == 1)
            {
                slider.value += 1;
            }
            else
            {
                slider.value -= 1;
            }

            if (slider.value <= 0 || slider.value >= 20)
            {
                EndGame();
            }

        }
    }

    private void EndGame()
    {
        GameObject particle = Instantiate(endParticle, new Vector3(0, 3, 0), Quaternion.identity);
        particle.transform.localScale = Vector3.one * 3;

        Vector3 explosionPos = Vector3.zero;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius * 100);
        foreach (Collider hit in colliders)
        {

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                cam.GetComponent<VitaminShake>().shakeDuration = 1f;
                cam.GetComponent<VitaminShake>().shakeAmount = 3f;
                rb.AddExplosionForce(power * 10, new Vector3(explosionPos.x, explosionPos.y, explosionPos.z), radius * 10, upforce * 10, ForceMode.Impulse);
            }
            if (slider.value == 20)
            {
                winners[1].SetActive(true);
                StartCoroutine(WaitForMenu());
            }
            if (slider.value == 0)
            {
                winners[0].SetActive(true);
                StartCoroutine(WaitForMenu());
            }
        }
    }

    private IEnumerator WaitForMenu()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator WaitForSong()
    {
        yield return new WaitForSeconds(music.GetComponent<AudioSource>().clip.length);
        music.GetComponent<AudioSource>().clip = headache;
        music.GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            players.Add(other.gameObject);
            other.gameObject.GetComponent<PlayerController>().hand = this;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(players.Contains(other.gameObject))
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();

            if(pc.hasPill)
            {
                pc.canSlamDunk = true;
            }
            else
            {
                pc.canSlamDunk = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (players.Contains(other.gameObject))
        {
            other.gameObject.GetComponent<PlayerController>().canSlamDunk = false;
            other.gameObject.GetComponent<PlayerController>().hand = null;
            players.Remove(other.gameObject);
        }
    }

}
