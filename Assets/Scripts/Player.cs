using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] Transform feet;
    [SerializeField] Rigidbody2D rb;

    [Space]

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] float xDirection, moveSpeed;
    [SerializeField] float jumpHeight, upGravity, downGravity;
    [SerializeField] float coyoteTime;
    float coyoteTimeCounter;
    [SerializeField] float jumpBufferTime;
    float jumpBufferCounter;

    InputActionPhase jumpPhase;

    Vector2 moveDirection = Vector2.right;
    bool isGrounded, canJump;

    private void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(feet.position, groundCheckSize, 0f, groundLayer);

        Vector2 v = rb.velocity;
        v.x = xDirection * moveSpeed;

        rb.gravityScale = rb.velocity.y < 0f ? downGravity : upGravity;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpPhase == InputActionPhase.Performed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            v.y = Mathf.Sqrt(2f * jumpHeight * Physics2D.gravity.magnitude * rb.gravityScale);
            
            jumpBufferCounter = 0f;
        }

        if (jumpPhase == InputActionPhase.Performed && v.y > 0f)
        {
            coyoteTimeCounter = 0f;
        }

        rb.velocity = v;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        jumpPhase = ctx.phase;
    }

    void FlipDirection()
    {
        xDirection *= -1;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Mathf.Abs(collision.contacts[0].point.x - rb.position.x) > 0.45f)
        {
            FlipDirection();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feet.position, groundCheckSize);
    }
}
