using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEditor.Timeline.Actions;

public class PlayerMovement2D : MonoBehaviour
{
    private CustomInput input = null;
    public float moveSpeed;
    private float dashSpeed = 1;
    public float dashSpeedMult;
    public float dashCooldown = 0.5f;
    bool canDash = true;
    bool canMove = false;
    bool lockedDirection = false;
    public int dashTime;
    public int m_dashTime;
    public Rigidbody2D rb;
    private Vector2 movement = Vector2.zero;

    private void Awake()
    {
        input = new CustomInput();
        moveSpeed = FindAnyObjectByType<Player>().moveSpeed;
        canMove = true;
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Dash.performed += OnDashPerformed;
        input.Player.Dash.canceled += OnDashCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Dash.performed -= OnDashPerformed;
        input.Player.Dash.performed -= OnDashCancelled;
    }
    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        if (dashTime > 0)
        {
            dashTime--;
        }
        //rb.MovePosition(rb.position + (movement * moveSpeed * Time.fixedDeltaTime) * dashSpeed);
        rb.velocity = movement * moveSpeed * dashSpeed;
    }
    public void LockMovement()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
    }
    public void UnlockMovement()
    {
        canMove = true;
    }
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (lockedDirection)
        {
            return;
        }
        movement = value.ReadValue<Vector2>();
        
    }
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        if (lockedDirection)
        {
            return;
        }
        movement = Vector2.zero;
    }
    private void OnDashPerformed(InputAction.CallbackContext value)
    {
        if (canDash) 
        { 
            if (movement != Vector2.zero) 
            { 
                dashSpeed *= dashSpeedMult;
                dashTime = m_dashTime; 
                lockedDirection = true; 
                StartCoroutine(ResetDash());
                canDash = false;
            }
        }

    }
    private void OnDashCancelled(InputAction.CallbackContext value)
    {
        
    }
    private IEnumerator ResetDash()
    {
        yield return new WaitUntil(() => dashTime <= 1);
        dashSpeed = 1;
        lockedDirection = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }
}
