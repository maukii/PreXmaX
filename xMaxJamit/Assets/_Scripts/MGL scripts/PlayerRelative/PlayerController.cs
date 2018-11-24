using System;
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
    Animator anim;

    Vector3 cameraForward = Vector3.zero;
    Vector3 cameraRight = Vector3.zero;
    Vector3 desiredDirection = Vector3.zero;

    public bool canSlamDunk;
    public bool hasPill;
    public bool stunned;
    bool slamming;
    public bool touchingFloor;

    public GameObject pillInHand { get; set; }
    public GodHand hand { get; set; }

    float timer;
    public bool canInteract = true;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        cameraForward = Camera.main.transform.forward;
        cameraRight = Camera.main.transform.right;

        cameraForward.y = cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();
    }

    private void Update()
    {
        Inputs();
        InputsAndMovement();
    }

    private void FixedUpdate()
    {
        UpdateAnimatior();

        if (stunned)
        {
            anim.SetFloat("Input", 0);
            rb.constraints = RigidbodyConstraints.None;

            if (timer <= 0f)
            {
                stunned = false;
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                ResetPlayerRotation();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void InputsAndMovement()
    {
        if (desiredDirection.magnitude > 0.2f && !stunned && !slamming)
        {
            DoPlayerMovement(desiredDirection);
            RotatePlayerModel(desiredDirection);
        }

        if ((Input.GetKeyDown(KeyCode.Joystick1Button0) && playerNumber == 1) || (Input.GetKeyDown(KeyCode.Joystick2Button0) && playerNumber == 2) && canInteract)
        {
            if (interaction.pills.Count > 0 && !hasPill)
            {
                if (!interaction.pills[0].GetComponent<VitaminDestroy>().hasThrown)
                {
                    PickUp(interaction.pills[0]);
                }
            }
            else if (hasPill)
            {
                if (canSlamDunk)
                {
                    SlamDunk(pillInHand);
                }
                else
                {
                    Throw(pillInHand);
                }
            }
            canInteract = false;
        }

        if ((Input.GetKeyUp(KeyCode.Joystick1Button0) && playerNumber == 1) || (Input.GetKeyUp(KeyCode.Joystick2Button0) && playerNumber == 2))
        {
            canInteract = true;
        }

    }

    private void UpdateAnimatior()
    {
        anim.SetFloat("Input", desiredDirection.magnitude);
        anim.SetBool("hasPill", hasPill);
    }

    private void Inputs()
    {
        float hor = Input.GetAxis("Joy" + playerNumber.ToString() + "X");
        float ver = -Input.GetAxis("Joy" + playerNumber.ToString() + "Y");

        desiredDirection = cameraForward * ver + cameraRight * hor;
    }

    private void PickUp(GameObject pill)
    {
        interaction.pills.Remove(interaction.pills[0]);

        pillInHand = pill;
        pill.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        pillInHand.GetComponent<VitaminDestroy>().timerActive = false;
        pillInHand.GetComponent<Rigidbody>().isKinematic = true;
        pillInHand.transform.position = pillPosition.position;
        pillInHand.transform.parent = pillPosition.transform;
        hasPill = true;
    }

    private void Throw(GameObject pill)
    {
        anim.SetTrigger("Throw");

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

    public void SlamDunk(GameObject pill)
    {
        anim.SetTrigger("Dunk");
        hasPill = false;
        canSlamDunk = false;
        slamming = true;
        StartCoroutine(DunkLogic(pill));
    }

    IEnumerator DunkLogic(GameObject pill)
    {
        yield return new WaitForSeconds(1);

        if(pillInHand != null)
        {
            hand.GetComponent<GodHand>().GivePill(pill, playerNumber, otherPlayer);
        }

        pillInHand = null;
        Destroy(pill);
        slamming = false;
    }

    public void Stun(float dur)
    {        
        rb.constraints = RigidbodyConstraints.None;

        timer += dur;

        if (timer > 3f)
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

    public void ResetPlayerRotation()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }

    private void OnDrawGizmos()
    {
        float distance = 2f;
        Vector3 direction = desiredDirection;
        Vector3 point_C = transform.position + (direction.normalized * distance);
        Debug.DrawLine(transform.position, point_C, Color.cyan);
    }

}
