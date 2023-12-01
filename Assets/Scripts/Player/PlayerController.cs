using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(GroundCheck))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speedChangeRate;
    [SerializeField] private float JumpTimeout = 0.50f;
    [SerializeField] private float smoothRotation;

    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private bool isGrounded;
    [SerializeField] private float currentSpeed;
    private float jumpTimeoutDelta;
    private Rigidbody rb;
    private GroundCheck groundCheck;
    private PlayerInputHandler input;

    //Events
    public event Action<float> OnSpeedChangeAction;
    public event Action<bool> OnGroundStateChangeAction;
    public event Action<bool> OnJumpAction;
    public event Action<bool> OnFreeFallAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = GetComponent<GroundCheck>();
        input = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        // subscribe events
        groundCheck.OnIsGroundedChangeAction += GroundCheck_OnIsGroundedChangeAction;

        // reset timeouts on start
        jumpTimeoutDelta = JumpTimeout;
    }

    private void GroundCheck_OnIsGroundedChangeAction(bool isGrounded)
    {
        this.isGrounded = isGrounded;
        OnGroundStateChangeAction?.Invoke(isGrounded);
    }

    private void Update()
    {
        HandleMovement();
        Rotate();
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleMovement()
    {
        float targetSpeed = input.sprint ? sprintSpeed : walkSpeed;

        if (input.move == Vector2.zero) targetSpeed = 0f;

        float speedOffset = 0.1f;

        if (targetSpeed > speedOffset)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        }
        else
        {
            // If currentSpeed is greater than the speedOffset, reduce it, otherwise set it directly to 0
            currentSpeed = currentSpeed > speedOffset
                ? Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * speedChangeRate)
                : 0f;
        }

        OnSpeedChangeAction?.Invoke(currentSpeed);
    }

    private void Rotate()
    {
        // normalise input direction
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // if there is a move input rotate player when the player is moving
        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                smoothRotation);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

    private void Move()
    {
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        rb.velocity = new Vector3(targetDirection.x, 0, targetDirection.z) * currentSpeed * Time.fixedDeltaTime +
            new Vector3(0, rb.velocity.y, 0);

    }


    private void Jump()
    {
        if (isGrounded)
        {
            OnJumpAction?.Invoke(false);
            OnFreeFallAction?.Invoke(false);

            if (input.jump && jumpTimeoutDelta <= 0.0f)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                OnJumpAction?.Invoke(true);

                input.jump = false;
            }

            // jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.fixedDeltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeoutDelta = JumpTimeout;

            if (rb.velocity.y <= 0f)
            {
                OnFreeFallAction?.Invoke(true);
            }

            input.jump = false;
        }
    }
}
