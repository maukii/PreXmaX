using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminGod : MonoBehaviour {

    int vitaminNumber;
    [SerializeField] GameObject[] objective;

	void Start ()
    {
        GetVitamin();
	}

    public void GetVitamin()
    {
        vitaminNumber = Random.Range(1, 4);
        objective[0].SetActive(false);
        objective[1].SetActive(false);
        objective[2].SetActive(false);
        Vitamins();
    }
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GetVitamin();
        }
    }

    void Vitamins()
    {
        if (vitaminNumber == 1)
        {
            objective[0].SetActive(true);
            objective[1].SetActive(false);
            objective[2].SetActive(false);
        }
        else if (vitaminNumber == 2)
        {
            objective[0].SetActive(false);
            objective[1].SetActive(true);
            objective[2].SetActive(false);
        }
        else if (vitaminNumber == 3)
        {
            objective[0].SetActive(false);
            objective[2].SetActive(false);
            objective[2].SetActive(true);
        }
        else
        {
            Debug.Log("WTFBBQ");
        }
    }
}