using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    //public float jumpForce = 5f;
    //public float jumpCooldown = 0.5f;
    public float airMultiplier = 0.5f;
    public float sprintMultiplier = 1.8f;
    //bool jumpHeld;

    public float waterDrag = 4f;
    public float airDrag = 0f;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrain = 1f;
    public float staminaRegen = 0.5f;

    [Header("Checks")]
    public LayerMask whatIsGround;
    public LayerMask whatIsWater;
    public LayerMask whatIsKelp;
    public LayerMask whatIsAirHole;
    public float groundCheckDistance = 0.6f; // adjust to seal height
    public Transform orientation;

    //[Header("Input")]
    //public KeyCode jumpKey = KeyCode.Space;

    public OrcaDeviation orca;

    Rigidbody rb;
    //bool readyToJump = true;
    bool isGrounded;
    bool isInWater;
    public bool IsHidden;
    int waterContacts = 0;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    public bool isSprinting;
    float currentStamina;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        bool sprintKey = Input.GetKey(KeyCode.LeftShift);

        if (sprintKey && currentStamina > 0f)
            isSprinting = true;
        else
            isSprinting = false;

        if (isSprinting)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isSprinting = false;
            }
        }
        else
        {
            if (currentStamina < maxStamina)
                currentStamina += staminaRegen * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }

        //jumpHeld = Input.GetKey(jumpKey) || Input.GetButton("Jump");

        float height = GetComponent<Collider>().bounds.extents.y;

        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            height + 0.1f,
            whatIsGround);

        rb.drag = isInWater ? waterDrag : airDrag;
        rb.useGravity = true;

        /*if (jumpHeld && readyToJump && isGrounded && !isInWater)
        {
            Jump();
        }*/

        SpeedControl();
    }
    private void FixedUpdate()
    {
        MovePlayer();
        if (isInWater)
        {
            rb.AddForce(Vector3.up * 20f, ForceMode.Force);
        }
    }
    private void MovePlayer()
    {
        float speedMultiplier = isSprinting ? sprintMultiplier : 1f;

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

            rb.AddForce(moveDirection.normalized * moveSpeed * 4f * speedMultiplier, ForceMode.Acceleration);
        }

        else if (isGrounded)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * moveSpeed * 7f * speedMultiplier, ForceMode.Force);
        }
        else
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * moveSpeed * 7f * airMultiplier * speedMultiplier, ForceMode.Force);
        }
    }
    /*private void Jump()
    {
        readyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump() => readyToJump = true;
    */
    private void SpeedControl()
    {
        float speedMultiplier = isSprinting ? sprintMultiplier : 1f;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed * speedMultiplier)
            rb.velocity = new Vector3(flatVel.normalized.x * moveSpeed * speedMultiplier, rb.velocity.y, flatVel.normalized.z * moveSpeed * speedMultiplier);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
        {
            waterContacts++;
            isInWater = true;
        }
        if (((1 << other.gameObject.layer) & whatIsKelp) != 0)
        {
            if (orca != null && !orca.chasing)
                IsHidden = true;
        }
        if (((1 << other.gameObject.layer) & whatIsAirHole) != 0)
            IsHidden = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
        {
            waterContacts--;
            if (waterContacts <= 0)
            {
                waterContacts = 0;
                isInWater = false;
            }
        }
        if (((1 << other.gameObject.layer) & whatIsKelp) != 0)
            IsHidden = false;

        if (((1 << other.gameObject.layer) & whatIsAirHole) != 0)
            IsHidden = false;
    }
}