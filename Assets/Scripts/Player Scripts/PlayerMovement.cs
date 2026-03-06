using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 1.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private CapsuleCollider2D playerCol;
    private SpriteRenderer spriteRenderer;
    private PlayerAnimation playerAnimation;
    public ParticleSystem jumpParticles;
    private float horizontalInput;

    private bool isHiding;
    private bool isCrouching;
    private Vector2 autoMoveTarget;
    private bool isAutoMoving = false;

    private float currentSpeed;
    private float originalHeight;
    private float crouchHeight;
    private float originalGravityScale;
    private Color originalColor;
    private readonly Color hidingColor = new Color(1f, 1f, 1f, 0.5f);


    public float MoveSpeedCopy => moveSpeed;
    public float HideSpeedCopy => crouchSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<PlayerAnimation>();
        crouchSpeed = moveSpeed / 2f;
        currentSpeed = moveSpeed;
        originalHeight = playerCol.size.y;
        crouchHeight = originalHeight / 2f;
        originalGravityScale = rb.gravityScale;
        originalColor = spriteRenderer.color;
        jumpParticles = GetComponentInChildren<ParticleSystem>();
    }

    public Vector2 GetMovementDirection()
    {
        return new Vector2(horizontalInput, 0f);
    }

    public void OnMove(InputValue input)
    {
        // Debug.Log(input);
        if (GameState.IsGamePaused() || isHiding || input == null)
        {
            horizontalInput = 0f;
            return;
        }

        Vector2 move = input.Get<Vector2>();
        horizontalInput = move.x;
        // jumpParticles.Play();

    }

    public void OnJump(InputValue input)
    {
        if (!input.isPressed || GameState.IsGamePaused() || isHiding || isCrouching || !IsGrounded()) { return; }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (playerAnimation != null)
        {
            playerAnimation.PlaySfx("Jump");
            jumpParticles.Play();
        }
    }

    public void OnCrouch(InputValue input)
    {
        if (!input.isPressed || GameState.IsGamePaused() || isHiding || !IsGrounded()) { return; }
        isCrouching = !isCrouching;
        playerCol.size = new Vector2(playerCol.size.x, isCrouching ? crouchHeight : originalHeight);

        if (playerAnimation != null)
        {
            playerAnimation.PlaySfx(isCrouching ? "Crouch" : "Uncrouch");
        }
    }

    public void OnHide(InputValue input)
    {
        if (!input.isPressed || GameState.IsGamePaused()) { return; }
        if (!isHiding && !BehindHidable())
        {
            Debug.Log("Not behind hidable object, cannot hide.");
            return;
        }


        // Toggle hiding state
        // Unhide
        if (isHiding)
        {
            isHiding = false;
            spriteRenderer.color = originalColor;
            playerCol.enabled = true;
            rb.gravityScale = originalGravityScale;

            if (playerAnimation != null)
            {
                playerAnimation.PlaySfx("Unhide");
            }
        }
        // Hide
        else
        {
            isHiding = true;
            spriteRenderer.color = hidingColor;
            playerCol.enabled = false;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;

            if (playerAnimation != null)
            {
                playerAnimation.PlaySfx("Hide");
            }
        }
    }

    public bool IsGrounded()
    {
        Vector2 origin = transform.position;
        RaycastHit2D hitCenter = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer | obstacleLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin + (Vector2.left * playerCol.bounds.extents.x), Vector2.down, groundCheckDistance, groundLayer | obstacleLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin + (Vector2.right * playerCol.bounds.extents.x), Vector2.down, groundCheckDistance, groundLayer | obstacleLayer);
        return hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null;
    }

    private bool BehindHidable()
    {
        // Check if player is touching a hidable object 
        Vector2 position = transform.position;
        float radius = 0.3f;                     // ! Adjust this as needed for player size
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Hidable"))
            {
                Debug.Log("Player is behind a hidable object: " + collider.name);
                return true;
            }
        }
        return false; // gives an error without this
    }

    public bool IsHiding()
    {
        return isHiding;
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    // for the door animation
    public bool IsAutoMoving()
    {
        return isAutoMoving;
    }
    public void MoveToPosition(Vector2 target)
    // allows external scripts to set a target position for the player to auto-move towards (used in door script to walk into the door)
    {
        autoMoveTarget = target;
        isAutoMoving = true;
    }

    private void FixedUpdate()
    {
        if (isHiding || GameState.IsGamePaused()) //GameState.IsPaused
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 velocity = rb.linearVelocity;
        currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

        // Non-player-controlled movement (for door animation)
        if (isAutoMoving)
        {

            Vector2 dir = autoMoveTarget - (Vector2)transform.position;
            if (Mathf.Abs(dir.x) < 0.05f)
            {
                isAutoMoving = false;
                velocity.x = 0f;
            }
            else
            {
                velocity.x = Mathf.Sign(dir.x) * currentSpeed;
            }
        }
        // Normal player-controlled movement
        else
        {
            velocity.x = horizontalInput * currentSpeed;
        }
        rb.linearVelocity = velocity;
    }
}
