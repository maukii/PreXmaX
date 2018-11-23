using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int playerNumber = 1;

    [Header("Movement Variables")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 200f;

    [SerializeField] GameObject graphics;

    #region PrivateVariables

    Vector3 cameraForward = Vector3.zero;
    Vector3 cameraRight = Vector3.zero;

    #endregion

    private void Start()
    {
        cameraForward = Camera.main.transform.forward;
        cameraRight = Camera.main.transform.right;

        cameraForward.y = cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();
    }

    private void FixedUpdate()
    {

        float hor = Input.GetAxis("Joy" + playerNumber.ToString() + "X");
        float ver = -Input.GetAxis("Joy" + playerNumber.ToString() + "Y");

        Vector3 desiredDirection = cameraForward * ver + cameraRight * hor;
        DoPlayerMovement(desiredDirection);

        if(desiredDirection != Vector3.zero)
        {
            RotatePlayerModel(desiredDirection);
        }

    }

    private void DoPlayerMovement(Vector3 desiredDirection)
    {
        transform.Translate(desiredDirection * movementSpeed * Time.deltaTime);
    }

    private void RotatePlayerModel(Vector3 desiredDirection)
    {
        graphics.transform.rotation = Quaternion.LookRotation(desiredDirection);
    }
}
