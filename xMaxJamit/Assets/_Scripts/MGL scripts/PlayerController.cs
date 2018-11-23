using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int playerNumber = 1;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 200f;

    public float hor, ver;

    private void FixedUpdate()
    {

        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        Debug.Log("Horizontal" + playerNumber.ToString() + " " + "Vertical" + playerNumber.ToString());

        hor *= Time.deltaTime * rotationSpeed;
        ver *= Time.deltaTime * movementSpeed;

        transform.Translate(0, 0, ver);
        transform.Rotate(0, hor, 0);

    }
}
