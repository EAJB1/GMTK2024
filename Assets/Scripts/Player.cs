using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Transform feet;
    [SerializeField] Rigidbody2D rb;

    [Space]

    [SerializeField] LayerMask groundLayer, wallLayer;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] float xDirection, moveSpeed, jumpHeight, upGravity, downGravity;

    InputActionPhase jumpPhase;

    Vector2 moveDirection = Vector2.right;
    bool isGrounded;

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(feet.position, groundCheckSize, 0f, groundLayer);

        Vector2 v = rb.velocity;
        v.x = xDirection * moveSpeed;

        rb.gravityScale = rb.velocity.y < 0f ? downGravity : upGravity;

        if (isGrounded)
        {
            if (jumpPhase == InputActionPhase.Performed)
            {
                v.y = Mathf.Sqrt(2f * jumpHeight * rb.gravityScale);
            }
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
        if (wallLayer == (wallLayer | (1<<collision.gameObject.layer)))
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
