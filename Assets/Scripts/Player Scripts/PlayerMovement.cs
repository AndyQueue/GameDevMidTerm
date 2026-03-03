using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(GameUIManager))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2.5f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 1.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;
    // using hideSpeed for crouching 
    private float curSpeed;
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction crouchAction;
    // private InputAction uncrouchAction;
    private InputAction hideAction;
    private float horizontalInput;
    private bool jumpRequested;
    private bool CrouchRequested;
    // private bool UncrouchRequested;
    private bool hideRequested;
    private bool isHiding;
    private bool isCrouching;
    private Vector2 autoMoveTarget;
    private bool isAutoMoving = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor = Color.white;
    Color hidingColor = new Color(1f, 1f, 1f, 0.5f);
    private GameUIManager gameUIManagerScript;
    private CapsuleCollider2D playerCol;
    private Rigidbody2D rb;
    // public copies of private variables 
    public float MoveSpeed => moveSpeed;
    public float HideSpeed => crouchSpeed;
    private float originalHeight;
    private float crouchHeight;
    private float gravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCol = GetComponent<CapsuleCollider2D>();
        gameUIManagerScript = GetComponent<GameUIManager>();
        crouchSpeed = moveSpeed / 2f;
        curSpeed = moveSpeed;
        originalHeight = playerCol.size.y;
        crouchHeight = originalHeight / 2f;
        gravityScale = rb.gravityScale;

        if (playerInput == null)
        { Debug.LogError("PlayerInput component is missing on PlayerMovement GameObject."); return; }

        jumpAction = playerInput.actions.FindAction("Jump");
        crouchAction = playerInput.actions.FindAction("Crouch");
        // uncrouchAction = playerInput.actions.FindAction("Uncrouch");
        hideAction = playerInput.actions.FindAction("Hide");

        if (jumpAction == null) { Debug.LogError("Input action 'Jump' not found in PlayerInput actions."); }
        if (crouchAction == null) { Debug.LogError("Input action 'Crouch' not found in PlayerInput actions."); }
        if (hideAction == null) { Debug.LogError("Input action 'Hide' not found in PlayerInput actions."); }
    }

    public void OnMove(InputValue input)
    {
        // Only use horizontal input for movement; up/down reserved for jump/crouch
        Vector2 move = input.Get<Vector2>();
        horizontalInput = move.x;
    }

    public Vector2 GetMovementDirection()
    {
        return new Vector2(horizontalInput, 0f);
    }

    public bool IsGrounded()
    {
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer | obstacleLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin + (Vector2.left * playerCol.bounds.extents.x), Vector2.down, groundCheckDistance, groundLayer | obstacleLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin + (Vector2.right * playerCol.bounds.extents.x), Vector2.down, groundCheckDistance, groundLayer | obstacleLayer);
        bool grounded = hit.collider != null || hitLeft.collider != null || hitRight.collider != null;
        return grounded;
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

    private void HandleJump(ref Vector2 velocity)
    {
        if (!jumpRequested) { return; }

        Debug.Log("Jump pressed");
        jumpRequested = false; // consume the jump
        velocity.y = jumpForce;

    }

    // Crouching toggle 
    private void HandleCrouchToggle()
    {
        if (!CrouchRequested) { return; }
        // Uncrouch if already crouching, otherwise crouch
        if (isCrouching)
        {
            Debug.Log("Uncrouch");
            CrouchRequested = false; // consume the Crouch
            isCrouching = false;
            if (playerCol != null) { playerCol.size = new Vector2(playerCol.size.x, originalHeight); }
            else
            {
                Debug.LogWarning("PlayerMovement: CapsuleCollider2D component not found on the GameObject.");
            }
        }
        else
        {
            Debug.Log("Crouch");
            CrouchRequested = false; // consume the Crouch
            isCrouching = true;
            // shrink collider height while crouching so the player appears smaller
            if (playerCol != null)
            { playerCol.size = new Vector2(playerCol.size.x, crouchHeight); }
            else
            {
                Debug.LogWarning("PlayerMovement: CapsuleCollider2D component not found on the GameObject.");
            }
        }
    }

    private bool BehindHidable()
    {
        // Check if player is touching a hidable object 
        Vector2 position = transform.position;
        float radius = 0.3f;                            // ! Adjust this as needed for player size
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
    private void HandleHideToggle()
    {
        // Debug.Log(hideRequested);
        if (!hideRequested) { return; }
        if (hideRequested && !BehindHidable())
        {
            Debug.Log("Not behind hidable");
            hideRequested = false; // consume the hide
            return;
        }
        if (hideRequested && !IsGrounded())
        {
            Debug.Log("Not grounded");
            return;
        }
        // Debug.Log("Hide pressed");

        // Toggle hide state
        // Unhide
        if (IsHiding())
        {
            Debug.Log("Unhide");
            hideRequested = false;                      // consume the hide
            isHiding = false;
            spriteRenderer.color = originalColor;       // Restore original color to unhide
            playerCol.enabled = true;                   // Re-enable collider to unhide
            rb.gravityScale = gravityScale;             // Restore gravity when unhiding
        }
        // Hide
        else
        {
            Debug.Log("Hide");
            hideRequested = false;                      // consume the hide
            isHiding = true;
            spriteRenderer.color = hidingColor;         // Set alpha to 0 to hide
            playerCol.enabled = false;                  // Disable collider to hide
            rb.gravityScale = 0f;                       // Disable gravity when hiding
        }
    }

    public void MoveToPosition(Vector2 target)
    // allows external scripts to set a target position for the player to auto-move towards (used in door script to walk into the door)
    {
        autoMoveTarget = target;
        isAutoMoving = true;
    }


    // Mainly used for sensing player input (updated every frame)
    private void Update()
    {
        // Read input every rendered frame
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && !jumpRequested && IsGrounded() && !isCrouching && !isHiding && !gameUIManagerScript.isPaused) { jumpRequested = true; }
        if (crouchAction != null && crouchAction.WasPressedThisFrame() && !CrouchRequested && IsGrounded() && !gameUIManagerScript.isPaused) { CrouchRequested = true; }
        if (hideAction != null && hideAction.WasPressedThisFrame() && !hideRequested && IsGrounded() && !gameUIManagerScript.isPaused) { hideRequested = true; }
    }
    // Mainly used for applying physics movement (updated at fixed time steps)
    private void FixedUpdate()
    {
        // Apply physics movement at fixed time steps
        // Determine current speed based on crouch/hide state
        curSpeed = isCrouching ? crouchSpeed : moveSpeed;

        Vector2 velocity = rb.linearVelocity;

        if (isAutoMoving)
        {
            // Debug.Log($"Auto-moving towards {autoMoveTarget} from {transform.position}");
            // Debug.Log($"Current velocity: {velocity}");
            Vector2 dir = autoMoveTarget - (Vector2)transform.position;
            if (Mathf.Abs(dir.x) < 0.05f)
            {
                isAutoMoving = false;
                velocity.x = 0f;
            }
            else
            {
                velocity.x = Mathf.Sign(dir.x) * curSpeed;
            }
        }
        else
        {
            velocity.x = horizontalInput * curSpeed;
        }
        if (gameUIManagerScript.isPaused)
        {
            return;
        }
        if (IsGrounded())
        {
            // crouch and uncrouch adjust player collider size and curSpeed to either moveSpeed or crouchSpeed
            HandleCrouchToggle();
            // grays out the character but should turn off the sprite and collider 
            HandleHideToggle();
            // jump preserves horizontal velocity
            if (!isCrouching && !isHiding) { HandleJump(ref velocity); }
        }
        // Debug.Log($"curSpeed: {curSpeed}, horizontalInput: {horizontalInput}, velocity: {velocity}");
        // Apply the final velocity to the Rigidbody

        if (!isHiding) { rb.linearVelocity = velocity; }


    }
    // Logic between Update and FixedUpdate: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Rigidbody-linearVelocity.html 
    // We should confirm with Benno.

}
