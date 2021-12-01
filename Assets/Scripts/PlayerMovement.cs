using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Player movement, jumping, slope checking. Following a Rigidbody Controller guide made by Plai.
    // I later added sounds and adjusted some values to better match TF2's physics

    private float playerHeight = 2f;

    [SerializeField] Transform orientation;

    public float moveSpeed = 6f;

    [Header("Movement")]
    private float horizontalMovement;
    private float verticalMovement;
    private float movementMultiplier = 7f;
    [SerializeField] float airMultiplier = 0.4f;
    private float groundDrag = 6f;
    private float airDrag = 2f;

    private bool isGrounded;
    public float jumpForce = 5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 moveDirection;
    private Vector3 slopeDirection;

    private Rigidbody rb;

    private RaycastHit slopeHit;

    [Header("Footsteps")]
    public AudioSource footstepsSource;
    public AudioClip[] footstepsArray;
    private float footstepTimer = 0.5f;

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                // If the raycast normal is NOT perpendicular, return true.
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        MyInput();
        ControlDrag();
        FootstepSound();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        slopeDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        footstepTimer = footstepTimer - Time.deltaTime;
    }

    private void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ControlDrag()
    {
        // Reduce drag if airborne for faster movement
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Adds force depending on if the player is on a slope, grounded, or mid-air
        if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        
    }

    private void FootstepSound()
    {
        // Play footsteps if grounded and holding WASD. Uses a timer to prevent spam.
        if( ( Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ) && isGrounded && footstepTimer <= 0)
        {
            footstepsSource.PlayOneShot(footstepsArray[Random.Range(0, footstepsArray.Length)]);
            footstepTimer = 0.5f;
        }
    }
}
