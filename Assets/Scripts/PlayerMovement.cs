using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    private int directionFacing = 1; //1 is right, -1 is left 
    [SerializeField] private int dashSpeedX;
    [SerializeField] private int dashSpeedY;
    [SerializeField] private int dashSpeedMult;
    private Animator animator;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool canJump = false;
    private Rigidbody2D rb;
    [SerializeField] Vector2 vel;
    [SerializeField] private bool m_Grounded;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private float k_GroundedRadius;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private int jumpGroundedFix;
    [SerializeField] private int landingDelayTime;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private bool canDash = false;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        input = new CustomInput();
    }
    private void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
        jumpGroundedFix = 0;
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCancelled;
        input.Player.Dash.performed += OnDashPerformed;
        input.Player.Dash.canceled += OnDashCancelled;
    }
    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCancelled;
        input.Player.Dash.performed -= OnDashPerformed;
        input.Player.Dash.canceled -= OnDashCancelled;
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2((dashSpeedX * directionFacing) + (moveSpeed * moveVector.x),1) * new Vector2(1,rb.velocity.y);
        transform.localScale = new Vector2(directionFacing,transform.localScale.y);
        if (moveVector !=  Vector2.zero) { animator.SetBool("IsMoving", true); } //allows animator to trigger
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
        vel = rb.velocity;

        if (jumpGroundedFix > 0)
        {
            jumpGroundedFix -= 1;
        }

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && jumpGroundedFix == 0)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    animator.SetBool("IsGrounded", true);
                    SetGrounded(true);
                }
                
            }
            
        }
    }
    private void SetGrounded(bool value)
    {
        isGrounded = value;
        canJump = value;
        canDash = value;
    }
    IEnumerator SetGroundedAfterSeconds(bool value, float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
        if (value) { isGrounded = true; canDash = true; canJump = true; } else if (!value){ isGrounded = false; }
    }
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        if (value.ReadValue<Vector2>().x != 0) 
        {
            if (value.ReadValue<Vector2>().x > 0) { directionFacing = 1; } else { directionFacing = -1; }
        }
    }
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (isGrounded && canJump)
        {
            canJump = false;
            isGrounded = false;
            Debug.Log("Jump Performed");
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpGroundedFix = landingDelayTime;
        }        
    }
    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        if (rb.velocity.y > 0) { rb.velocity = new Vector2(rb.velocity.x, 0); }
        Debug.Log("Jump Cancelled");
    }
    private void OnDashPerformed(InputAction.CallbackContext value)
    {
        if (canDash)
        {
            dashSpeedX = dashSpeedMult;
            rb.velocity = new Vector2(rb.velocity.x, 0 + dashSpeedY);
            Debug.Log("Dash Performed");
            canDash = false;
        }
    }
    private void OnDashCancelled(InputAction.CallbackContext value)
    {
        dashSpeedX = 0;
        Debug.Log("Dash Cancelled");
    }
}
