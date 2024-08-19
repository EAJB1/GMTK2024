using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] Texture2D pointCursor, grabCursor;
    [SerializeField] Vector2 cursorHotspot;
    [SerializeField] float cursorTextureTime;

    [Space]

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator anim;
    [SerializeField] Transform feet, shield;
    [SerializeField] Rigidbody2D rb;

    [Space]

    [SerializeField] LayerMask groundLayer, wallLayer;
    [SerializeField] Vector2 groundCheckSize, shieldCheckSize;
    public float xDirection;
    [SerializeField] float moveSpeed;
    float previousXDirection;
    [SerializeField] float jumpHeight, upGravity, downGravity;
    [SerializeField] float coyoteTime;
    float coyoteTimeCounter;
    [SerializeField] float jumpBufferTime;
    float jumpBufferCounter;

    [Space]

    [SerializeField] Checkpoint currentCheckpoint;
    bool waitingForInput;

    InputActionPhase jumpPhase, selectPhase;
    bool midJump, clicked;

    InputActionPhase pausePhase, resumePhase;
    public bool gamePaused;

    Vector2 moveDirection = Vector2.right;
    bool isGrounded = true, canJump;

    ScaleableEntity[] scaleableEntities;


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

    private void Start()
    {
        Cursor.SetCursor(pointCursor, cursorHotspot, CursorMode.Auto);
        anim.SetBool("Running", true);
    }

    private void Update()
    {
        if (selectPhase == InputActionPhase.Performed)
        {
            //clicked = true;
        }

        if (jumpPhase == InputActionPhase.Performed)
        {
            jumpBufferCounter = jumpBufferTime;

            jumpPhase = InputActionPhase.Canceled;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            jumpBufferCounter = Mathf.Clamp(jumpBufferCounter, 0f, jumpBufferTime);
        }

        if (waitingForInput && selectPhase == InputActionPhase.Performed ||
            waitingForInput && jumpPhase == InputActionPhase.Performed)
        {
            StartPlayer();
            // display text: "Click to continue"
        }
    }

    private void LateUpdate()
    {
        clicked = false;
    }

    void FixedUpdate()
    {
        if(transform.parent != null)
        {
            Debug.Log(transform.parent);
        }

        if(Physics2D.OverlapBox(new Vector2(transform.position.x + xDirection * shield.localPosition.x, shield.position.y), shieldCheckSize, 0f, wallLayer))
        {
            SoundManager.instance.PlaySound("Shield Hit");

            FlipDirection();
        }

        Vector2 v = rb.velocity;

        if(v.y <= 5f)
        {
            midJump = false;
        }

        bool wasGrounded = isGrounded;
        isGrounded = !midJump && Physics2D.OverlapBox(feet.position, groundCheckSize, 0f, groundLayer);

        if(!wasGrounded && isGrounded)
        {
            SoundManager.instance.PlaySound("Land");
        }

        v.x = xDirection * moveSpeed;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
            coyoteTimeCounter = Mathf.Clamp(coyoteTimeCounter, 0f, coyoteTime);
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            v.y = Mathf.Sqrt(2f * jumpHeight * Physics2D.gravity.magnitude * upGravity);

            midJump = true;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;

            SoundManager.instance.PlaySound("Jump");
        }

        rb.gravityScale = v.y <= 0f ? downGravity : upGravity;
        rb.velocity = v;

        anim.SetBool("Falling", !isGrounded && v.y < 0f);
        anim.SetBool("Jumping", !isGrounded && v.y >= 0f);
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (gamePaused) return;

        jumpPhase = ctx.phase;
    }

    public void Select(InputAction.CallbackContext ctx)
    {
        if (gamePaused) return;

        selectPhase = ctx.phase;

        if (selectPhase == InputActionPhase.Performed)
        {
            clicked = true;
            Cursor.SetCursor(grabCursor, cursorHotspot, CursorMode.Auto);
            StartCoroutine(CursorSelectWait());
        }
    }

    public bool PlayerClicked()
    {
        return clicked;
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        pausePhase = ctx.phase;

        if (pausePhase == InputActionPhase.Performed)
        {
            gamePaused = !gamePaused;
        }
    }

    IEnumerator CursorSelectWait()
    {
        yield return new WaitForSeconds(cursorTextureTime);
        Cursor.SetCursor(pointCursor, cursorHotspot, CursorMode.Auto);
    }

    void FlipDirection()
    {
        xDirection *= -1;
        sr.flipX = !sr.flipX;
    }

    public void Respawn()
    {
        scaleableEntities = FindObjectsByType<ScaleableEntity>(FindObjectsSortMode.None);

        foreach (ScaleableEntity entity in scaleableEntities)
        {
            entity.Interact();
        }

        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.transform.position;

            if (currentCheckpoint.startDirection == Checkpoint.StartDirection.Left &&
                xDirection == Mathf.Abs(xDirection) ||
                currentCheckpoint.startDirection == Checkpoint.StartDirection.Right &&
                xDirection == -Mathf.Abs(xDirection))
            {
                FlipDirection();
            }

            if (currentCheckpoint.startStationary)
            {
                StopPlayer();
            }

            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetCurrentCheckpoint(Checkpoint checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void PlayStepSound()
    {
        SoundManager.instance.PlaySound("Step");
    }

    void StopPlayer()
    {
        previousXDirection = xDirection;
        xDirection = 0f;
        anim.SetBool("Running", false);
        waitingForInput = true;
    }

    void StartPlayer()
    {
        xDirection = previousXDirection;
        anim.SetBool("Running", true);
        waitingForInput = false;
    }
}
