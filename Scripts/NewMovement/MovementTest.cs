using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;
    
    [Header("Movement")] [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;
    
    [SerializeField] private AudioClip jumpSound;
    private AudioSource audioSource;

    [Header("Sprinting")] [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")] public float jumpForce = 5f;
    [SerializeField] private float fallForce = 10;

    [Header("Keybinds")] [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")] [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")] [SerializeField]
    Transform groundCheck;

    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }
    public bool isStill { get; private set; }
    public bool isWalking { get; private set; }
    public bool isRunning { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;
    [Header("Animation")] [SerializeField] private Animator animator;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public float SlopeAngle()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2 / 2 + 0.5f))
        {
            // Calculate the angle between the surface normal and the up direction
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            // Calculate the dot product of the surface normal and the up direction
            float dotProduct = Vector3.Dot(hit.normal, Vector3.up);

            // If the dot product is negative, make the slope angle negative
            if (dotProduct < 0)
            {
                slopeAngle *= -1;
            }

            return slopeAngle;
        }

        return 0f;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

//        horizontalMovement = Input.GetAxisRaw("Horizontal"); // A, D, Left, Right, sol = -1, sağ = 1
    //   verticalMovement = Input.GetAxisRaw("Vertical"); // W, S, Up, Down, ileri = 1, geri = -1
    private void Update()
    {
        float actualSpeed = rb.velocity.magnitude;
        // Check if the character's speed is above a certain threshold
        if (actualSpeed > 0.1f)
        {
            // If the character's speed is above the threshold, set the "walking" boolean to true
            animator.SetBool("walking", true);
        }
        else
        {
            // If the character's speed is below the threshold, set the "walking" boolean to false
            animator.SetBool("walking", false);
        }

        if (horizontalMovement == 1)
        {
            animator.SetBool("D", true);
        }
        else
        {
            animator.SetBool("D", false);
        }
        
        if (horizontalMovement == -1)
        {
            animator.SetBool("A", true);
        }
        else
        {
            animator.SetBool("A", false);
        }
        
        if (verticalMovement == 1)
        {
            animator.SetBool("W", true);
        }
        else
        {
            animator.SetBool("W", false);
        }

        if (verticalMovement == -1)
        {
            animator.SetBool("S", true);
        }
        else
        {
            animator.SetBool("S", false);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        MyInput();
        ControlDrag();
        ControlSpeed();

        switch (moveSpeed)
        {
            case 0f when isGrounded:
                isStill = true;
                isRunning = false;
                isWalking = false;
                break;
            case <= 4f when isGrounded:
                isStill = false;
                isRunning = false;
                isWalking = true;
                break;
            case >= 6f when isGrounded:
                isStill = false;
                isRunning = true;
                isWalking = false;
                break;
        }


        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
            audioSource.PlayOneShot(jumpSound);
        }


        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal"); // A, D, Left, Right, sol = -1, sağ = 1
        verticalMovement = Input.GetAxisRaw("Vertical"); // W, S, Up, Down, ileri = 1, geri = -1

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            audioSource.volume = 0.5f;
            audioSource.pitch = UnityEngine.Random.Range(0.7f, 1f);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
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

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier,
                ForceMode.Acceleration);
            rb.AddForce(Vector3.down * fallForce , ForceMode.Acceleration);
        }
    }

    
}