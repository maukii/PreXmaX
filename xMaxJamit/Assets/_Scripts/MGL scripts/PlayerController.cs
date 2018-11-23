using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int playerNumber = 1;

    [Header("Player Variables")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] bool hasPill = false;
    [SerializeField] bool canSlamDunk = false;

    [SerializeField] GameObject graphics;
    [SerializeField] PlayerInteraction interaction;

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

        if((Input.GetKeyDown(KeyCode.Joystick1Button0) && playerNumber == 1) || (Input.GetKeyDown(KeyCode.Joystick2Button0) && playerNumber == 2))
        {
            Debug.Log("Joy" + playerNumber.ToString() + "Button0");

            if(interaction.pills.Count != 0 && !hasPill)
            {
                PickUp();
            }
            else if(hasPill)
            {
                if(canSlamDunk)
                {
                    SlamDunk();
                }
                else
                {
                    Throw();
                }
            }
        }

    }

    private void PickUp()
    {
        throw new NotImplementedException();
    }

    private void Throw()
    {
        throw new NotImplementedException();
    }

    private void SlamDunk()
    {
        throw new NotImplementedException();
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
