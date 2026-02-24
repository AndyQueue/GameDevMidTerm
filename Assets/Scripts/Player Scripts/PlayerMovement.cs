using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2.5f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    // using hideSpeed for both crouching and hiding
    private float curSpeed;
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction uncrouchAction;
    private InputAction hideAction;
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool jumpRequested;
    private bool CrouchRequested;
    private bool UncrouchRequested;
    private bool HideRequested;
    private bool isHiding;
    private bool isCrouching;
    private CapsuleCollider2D playerCol;
    // public copies of private variables for use in other scripts (e.g. Idlehiding)
    public float MoveSpeed => moveSpeed;
    public float HideSpeed => crouchSpeed;
    private float OriginalHeight;
    private float CrouchHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCol = GetComponent<CapsuleCollider2D>();
        crouchSpeed = moveSpeed / 2f;
        curSpeed = moveSpeed;
        OriginalHeight = playerCol.size.y;
        CrouchHeight = OriginalHeight / 2f;

        if (playerInput == null)
        { Debug.LogError("PlayerInput component is missing on PlayerMovement GameObject."); return; }

        jumpAction = playerInput.actions.FindAction("Jump");
        crouchAction = playerInput.actions.FindAction("Crouch");
        uncrouchAction = playerInput.actions.FindAction("Uncrouch");
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
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider != null ? Color.green : Color.red);
        return hit.collider != null;
    }
    public bool IsHiding() { return isHiding; }
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
            if (playerCol != null) { playerCol.size = new Vector2(playerCol.size.x, OriginalHeight); }
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
            { playerCol.size = new Vector2(playerCol.size.x, CrouchHeight); }
            else
            {
                Debug.LogWarning("PlayerMovement: CapsuleCollider2D component not found on the GameObject.");
            }
        }
    }
    private bool BehindHidable()
    {
        // Check if player is touching a hidable object (e.g. bush) using an overlap circle
        Vector2 position = transform.position;
        float radius = 0.1f; // Adjust as needed for player size
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Hidable"))
            {
                return true;
            }
        }
        return false;
    }
    private void HandleHideToggle()
    {
        // Debug.Log(HideRequested);
        if (!HideRequested) { return; }
        if (HideRequested && !BehindHidable()) 
        { 
            Debug.Log("Not behind hidable"); 
            HideRequested = false; // consume the hide
            return;
        }
        if (HideRequested && !IsGrounded())
        {
            Debug.Log("Not grounded");
            return;
        }
        Debug.Log("Hide pressed");

        // Toggle hide state
        // Unhide
        if (IsHiding())
        {
            Debug.Log("Unhide");
            HideRequested = false; // consume the hide
            isHiding = false;
            // Unhide logic is in PlayerHiding's OnStateExit 
        }
        // Hide
        else
        {
            Debug.Log("Hide");
            HideRequested = false; // consume the hide
            isHiding = true;
            // Hide logic is in PlayerHiding's OnStateEnter and OnStateUpdate 
        }
    }

    // Mainly used for sensing player input (updated every frame)
    private void Update()
    {
        // Read input every rendered frame
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && !jumpRequested && IsGrounded() && !isCrouching && !isHiding) { jumpRequested = true; }
        if (crouchAction != null && crouchAction.WasPressedThisFrame() && !CrouchRequested && IsGrounded()) { CrouchRequested = true; }
        if (uncrouchAction != null && uncrouchAction.WasPressedThisFrame() && !UncrouchRequested && IsGrounded()) { UncrouchRequested = true; }
        if (hideAction != null && hideAction.WasPressedThisFrame() && !HideRequested && IsGrounded()) { HideRequested = true; }
    }
    // Mainly used for applying physics movement (updated at fixed time steps)
    private void FixedUpdate()
    {
        // Apply physics movement at fixed time steps
        // Determine current speed based on crouch/hide state
        curSpeed = isCrouching ? crouchSpeed : moveSpeed;

        Vector2 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * curSpeed;
        if (IsGrounded())
        {
            // jump preserves horizontal velocity
            // crouch and uncrouch adjust player collider size and curSpeed to either moveSpeed or hideSpeed
            if (!isCrouching) { HandleCrouchToggle(); } else { HandleCrouchToggle(); }
            HandleHideToggle();
            if (!isCrouching && !isHiding) { HandleJump(ref velocity); }
        }
        // Debug.Log($"curSpeed: {curSpeed}, horizontalInput: {horizontalInput}, velocity: {velocity}");
        // Apply the final velocity to the Rigidbody

        if (!isHiding) { rb.linearVelocity = velocity; }
    }
    // Logic between Update and FixedUpdate: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Rigidbody-linearVelocity.html 
    // We should confirm with Benno.
}
