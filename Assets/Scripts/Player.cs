using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform feet, shield;
    [SerializeField] Rigidbody2D rb;

    [Space]

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckSize, shieldCheckSize;
    [SerializeField] float xDirection, moveSpeed;
    [SerializeField] float jumpHeight, upGravity, downGravity;
    [SerializeField] float coyoteTime;
    float coyoteTimeCounter;
    [SerializeField] float jumpBufferTime;
    float jumpBufferCounter;

    InputActionPhase jumpPhase;

    Vector2 moveDirection = Vector2.right;
    bool isGrounded, canJump;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feet.position, groundCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + xDirection * shield.localPosition.x, shield.position.y), shieldCheckSize);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (jumpPhase == InputActionPhase.Performed)
        {
            jumpBufferCounter = jumpBufferTime;

            jumpPhase = InputActionPhase.Canceled;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if(Physics2D.OverlapBox(new Vector2(transform.position.x + xDirection * shield.localPosition.x, shield.position.y), shieldCheckSize, 0f, groundLayer))
        {
            FlipDirection();
        }

        Vector2 v = rb.velocity;
        
        isGrounded = v.y <= 0f && Physics2D.OverlapBox(feet.position, groundCheckSize, 0f, groundLayer);

        v.x = xDirection * moveSpeed;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            v.y = Mathf.Sqrt(2f * jumpHeight * Physics2D.gravity.magnitude * upGravity);
            
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        rb.gravityScale = rb.velocity.y < 0f ? downGravity : upGravity;
        rb.velocity = v;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        jumpPhase = ctx.phase;
    }

    void FlipDirection()
    {
        xDirection *= -1;
        sr.flipX = !sr.flipX;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (Mathf.Abs(collision.contacts[0].point.x - rb.position.x) > 0.45f)
        {
            FlipDirection();
        }
    }*/
}
