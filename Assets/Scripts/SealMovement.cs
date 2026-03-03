using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpCooldown = 0.5f;
    public float airMultiplier = 0.5f;
    bool jumpHeld;

    public float waterDrag = 4f;
    public float airDrag = 0f;

    [Header("Checks")]
    public LayerMask whatIsGround;
    public LayerMask whatIsWater;
    public float groundCheckDistance = 0.6f; // adjust to seal height
    public Transform orientation;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;

    Rigidbody rb;
    bool readyToJump = true;
    bool isGrounded;
    bool isInWater;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        jumpHeld = Input.GetKey(jumpKey) || Input.GetButton("Jump");

        float height = GetComponent<Collider>().bounds.extents.y;

        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            height + 0.1f,
            whatIsGround);

        rb.drag = isInWater ? waterDrag : airDrag;
        rb.useGravity = true;

        if (jumpHeld && readyToJump && isGrounded && !isInWater)
        {
            Jump();
        }


        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if (isInWater)
        {
            // Counteract some gravity
            rb.AddForce(Vector3.up * 20f, ForceMode.Force);
        }
    }

    private void MovePlayer()
    {
        if (isInWater)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            float upwardInput = 0f;

            if (Input.GetKey(KeyCode.Space))
                upwardInput = 1f;
            else if (Input.GetKey(KeyCode.LeftControl))
                upwardInput = -1f;

            moveDirection =
                camForward * verticalInput +
                camRight * horizontalInput +
                Vector3.up * upwardInput;

            rb.AddForce(moveDirection.normalized * moveSpeed * 4f, ForceMode.Acceleration);
        }

        else if (isGrounded)
        {
            // Walking on land
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * moveSpeed * 7f, ForceMode.Force);
        }
        else
        {
            // Falling / air movement
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * moveSpeed * 7f * airMultiplier, ForceMode.Force);
        }
    }

    private void Jump()
    {
        readyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump() => readyToJump = true;

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
            rb.velocity = new Vector3(flatVel.normalized.x * moveSpeed, rb.velocity.y, flatVel.normalized.z * moveSpeed);
    }

    // Water detection using trigger colliders
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
            isInWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
            isInWater = false;
    }
}