﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int playerNumber = 1;

    [Header("Player Variables")]
    [SerializeField] float movementSpeed = 5f;

    [SerializeField] Transform pillPosition;
    [SerializeField] GameObject graphics;
    [SerializeField] PlayerInteraction interaction;
    [SerializeField] GameObject otherPlayer;

    #region PrivateVariables

    Rigidbody rb;

    Vector3 cameraForward = Vector3.zero;
    Vector3 cameraRight = Vector3.zero;
    Vector3 desiredDirection = Vector3.zero;

    public bool canSlamDunk { get; set; }
    public bool hasPill { get; set; }
    public bool stunned { get; set; }

    public GameObject pillInHand { get; set; }
    public GodHand hand { get; set; }

    float timer;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

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

        desiredDirection = cameraForward * ver + cameraRight * hor;

        if(stunned)
        {
            if(timer <= 0f)
            {
                stunned = false;
                ResetPlayerRotation();
                rb.velocity = Vector3.zero;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        if(desiredDirection.magnitude > 0.2f && !stunned)
        {
            DoPlayerMovement(desiredDirection);
            RotatePlayerModel(desiredDirection);
        }

        if((Input.GetKeyDown(KeyCode.Joystick1Button0) && playerNumber == 1) || (Input.GetKeyDown(KeyCode.Joystick2Button0) && playerNumber == 2))
        {
            if(interaction.pills.Count > 0 && !hasPill)
            {
                if(!interaction.pills[0].GetComponent<VitaminDestroy>().hasThrown)
                {
                    PickUp(interaction.pills[0]);
                }
            }
            else if(hasPill)
            {
                if(canSlamDunk)
                {
                    SlamDunk(pillInHand);
                }
                else
                {
                    Throw(pillInHand);
                }
            }
        }
    }

    private void PickUp(GameObject pill)
    {
        interaction.pills.Remove(interaction.pills[0]);

        pillInHand = pill;
        pillInHand.GetComponent<VitaminDestroy>().timerActive = false;
        pillInHand.GetComponent<Rigidbody>().isKinematic = true;
        pillInHand.transform.position = pillPosition.position;
        pillInHand.transform.parent = pillPosition.transform;
        hasPill = true;
    }

    private void Throw(GameObject pill)
    {
        pill.transform.parent = null;

        float throwHeight = 5f;
        Vector3 throwDirection = new Vector3((otherPlayer.transform.position.x - transform.position.x), throwHeight, (otherPlayer.transform.position.z - transform.position.z));

        pill.GetComponent<VitaminDestroy>().hasThrown = true;
        pill.GetComponent<Rigidbody>().isKinematic = false;
        pill.GetComponent<Rigidbody>().AddForce(throwDirection * 50);
        pill.gameObject.layer += 1;
        pill = null;
        hasPill = false;
    }

    private void SlamDunk(GameObject pill)
    {
        hand.GetComponent<GodHand>().GivePill(pill, playerNumber);
        Destroy(pill);
        pill = null;
        hasPill = false;
        canSlamDunk = false;
    }

    public void Stun(float dur)
    {
        timer += dur;

        if(timer > 3f)
        {
            timer = 3f;
        }

        stunned = true;
        hasPill = false;
        Destroy(pillInHand);
        pillInHand = null;
    }

    private void DoPlayerMovement(Vector3 desiredDirection)
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    private void RotatePlayerModel(Vector3 desiredDirection)
    {
        float rotationSpeed = 3f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDirection), Time.deltaTime * rotationSpeed);
    }

    public void ResetPlayerRotation() // call this after player gets stunned and gets up
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }

    private void OnDrawGizmos()
    {
        float distance = 15f;
        Vector3 direction = desiredDirection;
        Vector3 point_C = transform.position + (direction.normalized * distance);
        Debug.DrawLine(transform.position, point_C, Color.cyan);
    }
}
