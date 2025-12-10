using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float groundDrag = 4f;

    public float jumpForce = 8f;
    public float jumpCooldown = 0.25f;

    public float airControl = 1f;
    public float airResistance = 0.985f;

    private bool readyToJump;
    private float lastJumpTime = 0f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("References")]
    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);

        HandleInput();
        ControlSpeed();

        rb.linearDamping = grounded ? groundDrag : 0f;

        if (grounded && horizontalInput == 0 && verticalInput == 0)
        {
            Vector3 vel = rb.linearVelocity;
            vel.x *= 0.85f;
            vel.z *= 0.85f;
            rb.linearVelocity = vel;
        }

        if (!grounded)
        {
            Vector3 vel = rb.linearVelocity;
            vel.x *= airResistance;
            vel.z *= airResistance;
            rb.linearVelocity = vel;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        HandleStepClimb();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded && Time.time >= lastJumpTime + jumpCooldown)
        {
            readyToJump = false;
            lastJumpTime = Time.time;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveSpeed * airControl * moveDirection.normalized * (grounded ? 120f : 120f), ForceMode.Acceleration);
    }

    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed * 1.1f)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed * 1.1f;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (grounded)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset vertical velocity only when grounded

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleStepClimb()
    {
        if (!grounded) return; 

        float stepHeight = 0.3f; 
        float stepSmooth = 0.15f;

        Vector3 rayDir = new Vector3(orientation.forward.x, 0f, orientation.forward.z).normalized;

        RaycastHit lowerHit;
        Vector3 originLower = transform.position + Vector3.up * 0.05f;
        bool hitLower = Physics.Raycast(originLower, rayDir, out lowerHit, 0.6f);

        RaycastHit groundHit;
        Vector3 originGround = transform.position + Vector3.up * 0.01f;
        bool hitGround = Physics.Raycast(originGround, rayDir, out groundHit, 0.6f);

        if (hitLower || hitGround)
        {
            RaycastHit upperHit;
            Vector3 originUpper = transform.position + Vector3.up * stepHeight;
            if (!Physics.Raycast(originUpper, rayDir, out upperHit, 0.6f))
            {
                rb.position += Vector3.up * stepSmooth;
            }
        }
    }
}
