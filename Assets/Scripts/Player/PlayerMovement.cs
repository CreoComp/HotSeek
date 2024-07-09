using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float rotateSpeed = 5f;
    public float turnSmoothTime = 0.1f; // Время сглаживания поворота
    public float turnSpeed = 2f;
    public float runSpeed = 5f;
    public float rollSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float wallRunDuration = 1f;
    public float wallJumpForce = 10f;
    public float climbHeight = 1f; // Высота для взбирания
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask wallMask;
    public LayerMask climbMask;
    public Animator animator; 

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentDirection;
    private bool isGrounded;
    private bool isRunning;
    private bool isWallRunning;
    private float wallRunTime;
    private bool isRolling;
    private bool isJumping;
    private bool isClimbing;
    private bool isFalling;

    private float speedMove;

    private Vector3 hitClimbPoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
       // HandleWallRun();
        HandleClimb();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        if (isClimbing)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;
        animator.SetFloat("Speed", move.normalized.magnitude);


        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + Camera.main.transform.localEulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateSpeed, turnSmoothTime);

            // Вращаем модель персонажа в сторону движения
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Двигаем персонажа в направлении вращения
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * speedMove * Time.deltaTime); 
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero)
            {
                speedMove = runSpeed;
                isRunning = true;
            }
            else if (move != Vector3.zero)
            {
                speedMove = walkSpeed;
                isRunning = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && move != Vector3.zero && isGrounded)
            {
                StartCoroutine(PerformRoll());
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    IEnumerator JumpAnimation()
    {
        animator.SetTrigger("JumpStart");
        yield return new WaitForSeconds(0.233f);
        animator.SetTrigger("JumpLand");

    }

    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            StartCoroutine(JumpAnimation());
            isJumping = true;
        }

        if (!isGrounded && velocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
        }
        else if (!isGrounded && velocity.y < -0.1f)
        {
            animator.SetBool("isFalling", true);
        }
    }

    /*void HandleWallRun()
    {
        if (!isGrounded && Input.GetKey(KeyCode.W))
        {
            if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit, 1f, wallMask) ||
                Physics.Raycast(transform.position, -transform.right, out hit, 1f, wallMask))
            {
                if (wallRunTime < wallRunDuration)
                {
                    velocity.y = 0;
                    controller.Move(transform.forward * runSpeed * Time.deltaTime);
                    wallRunTime += Time.deltaTime;
                    isWallRunning = true;
                }
                else
                {
                    isWallRunning = false;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    controller.Move(-hit.normal * wallJumpForce * Time.deltaTime);
                    isWallRunning = false;
                }
            }
            else
            {
                isWallRunning = false;
                wallRunTime = 0;
            }
        }
        else
        {
            isWallRunning = false;
            wallRunTime = 0;
        }
    }*/

    void HandleClimb()
    {
        if (isClimbing && (isGrounded))
            return;

        Debug.DrawRay(transform.position + Vector3.up * climbHeight, transform.forward * 11f + transform.up);
        Debug.DrawRay(transform.position + Vector3.up * climbHeight, transform.forward * 11f - transform.up);

        bool isUpRaycast = false;
        bool isDownRaycast = false;

        if (Physics.Raycast(transform.position + Vector3.up * climbHeight, transform.forward * 11f + transform.up, out RaycastHit hit1, 1f))
        {
            if (hit1.distance < 1f && hit1.normal.y < 0.1f)
            {
                // Trigger climb animation and adjust player's position
                isUpRaycast = true;
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up * climbHeight, transform.forward * 11f - transform.up, out RaycastHit hit2, 1f))
        {
            if (hit2.distance < 1f && hit2.normal.y < 0.1f)
            {
                // Trigger climb animation and adjust player's position
                isDownRaycast = true;
            }
        }

        if (isDownRaycast && !isUpRaycast)
        {

            animator.SetTrigger("ClimbStart");
            StartCoroutine(PerformClimb((transform.position + Vector3.up * climbHeight) + transform.forward));
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWallRunning", isWallRunning);
        animator.SetBool("isRolling", isRolling);

        // Set the vertical velocity parameter
        animator.SetFloat("VerticalVelocity", velocity.y);

        if(isGrounded)
            animator.SetBool("isFalling", false);

        if (isGrounded && isJumping)
        {
            animator.SetTrigger("JumpLand");
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            isJumping = false;
            isFalling = true;
        }

        if(isGrounded && isFalling)
        {
            isFalling = false;
            //StartCoroutine(PerformRoll());
        }
    }

    System.Collections.IEnumerator PerformClimb(Vector3 targetPoint)
    {
        isClimbing = true;
        // Disable controller to avoid interference
        controller.enabled = false;

        // Move player to the target position (above the ledge)
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetPoint.x, targetPoint.y + controller.height / 10, targetPoint.z);

        float climbDuration = 1.1f; // Adjust this based on animation length
        float elapsedTime = 0f;

        while (elapsedTime < climbDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / climbDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        controller.enabled = true;
        animator.SetTrigger("ClimbEnd");
        isClimbing = false;
    }

    System.Collections.IEnumerator PerformRoll()
    {
        animator.SetTrigger("Roll");
        isRolling = true;
        float rollDuration = 1f / 90f; // Adjust this based on animation length
        for (int i = 0; i < 90; i++)
        {
            controller.Move(transform.forward * rollSpeed * rollDuration * (i / 90f));
            yield return new WaitForSeconds(rollDuration);
        }
        isRolling = false;
    }
}
