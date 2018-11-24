using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodHand : MonoBehaviour
{
    public int number = 1;
    public float power, radius, upforce;
    public GameObject happyParticle, madParticle;

    Slider slider;

    public List<GameObject> players = new List<GameObject>();
    public GameObject[] pills = new GameObject[3];

    public VitaminDestroy.PillColor wantedPillColor;
    public Transform pillPosition;

    private void Start()
    {
        slider = FindObjectOfType<Slider>();

        for (int i = 0; i < pills.Length; i++)
        {
            pills[i].gameObject.SetActive(false);
        }

        ChangePill();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangePill();
        }
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

    public void GivePill(GameObject pill, int playerNumber)
    {
        if(pill.GetComponent<VitaminDestroy>().CurrentColor == wantedPillColor)
        {
            GetHappy(playerNumber);
        }
        else
        {
            GetMad(playerNumber);
        }
    }

    public void GetHappy(int playerNumber)
    {
        Instantiate(happyParticle, transform.position, Quaternion.identity);

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
            print("happy me");
        }

        if(slider.value <= 0 || slider.value >= 20)
        {
            EndGame();
        }

        ChangePill();
    }

    public void GetMad(int playerNumber)
    {
        Instantiate(madParticle, transform.position, Quaternion.identity);

        if (number == playerNumber)
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
                    float stunDur = 3f;
                    hit.gameObject.GetComponent<PlayerController>().Stun(stunDur);
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

            print("mad me");
            ChangePill();
        }
        else
        {
            if(playerNumber == 1)
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
        throw new NotImplementedException();
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
