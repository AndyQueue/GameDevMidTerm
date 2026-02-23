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
    // using sneakSpeed for both crouching and sneaking
    private float curSpeed;
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction uncrouchAction;
    private InputAction sneakAction;
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool jumpRequested;
    private bool CrouchRequested;
    private bool UncrouchRequested;
    private bool SneakRequested;
    private bool isSneaking;
    private bool isCrouching;
    private CapsuleCollider2D playerCol;
    // public copies of private variables for use in other scripts (e.g. Idlesneaking)
    public float MoveSpeed => moveSpeed;
    public float SneakSpeed => crouchSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerCol = GetComponent<CapsuleCollider2D>();
        crouchSpeed = moveSpeed / 2f;
        curSpeed = moveSpeed;

        if (playerInput == null)
        { Debug.LogError("PlayerInput component is missing on PlayerMovement GameObject."); return; }

        jumpAction = playerInput.actions.FindAction("Jump");
        crouchAction = playerInput.actions.FindAction("Crouch");
        uncrouchAction = playerInput.actions.FindAction("Uncrouch");
        sneakAction = playerInput.actions.FindAction("Sneak");

        if (jumpAction == null) { Debug.LogError("Input action 'Jump' not found in PlayerInput actions."); }
        if (crouchAction == null) { Debug.LogError("Input action 'Crouch' not found in PlayerInput actions."); }
        if (sneakAction == null) { Debug.LogError("Input action 'Sneak' not found in PlayerInput actions."); }
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
    public bool IsSneaking() { return isSneaking; }
    private void HandleJump(ref Vector2 velocity)
    {
        if (!jumpRequested) { return; }

        Debug.Log("Jump pressed");
        jumpRequested = false; // consume the jump
        velocity.y = jumpForce;

    }

    // Crouching toggle 
    private void HandleCrouch()
    {
        if (!CrouchRequested) { return; }

        Debug.Log("Crouch pressed");
        CrouchRequested = false; // consume the Crouch
        isCrouching = true;

        // shrink collider height while crouching so the player appears smaller
        if (playerCol != null)
        { playerCol.size = new Vector2(playerCol.size.x, 0.5f); }
        else
        {
            Debug.LogWarning("PlayerMovement: CapsuleCollider2D component not found on the GameObject.");
        }
    }
    private void HandleUncrouch()
    {
        if (!UncrouchRequested) { return; }

        Debug.Log("Uncrouch pressed");
        UncrouchRequested = false; // consume the Crouch
        isCrouching = false;

        if (playerCol != null) { playerCol.size = new Vector2(playerCol.size.x, 1f); }
        else
        {
            Debug.LogWarning("PlayerMovement: CapsuleCollider2D component not found on the GameObject.");
        }
    }
    public void SetSneak(bool isSneaking)
    {
        this.isSneaking = isSneaking;
    }
    private void HandleSneak()
    {
        if (!SneakRequested) { return; }

        Debug.Log("Sneak pressed");
        SneakRequested = false;
        SetSneak(!isSneaking);
    }

    // Mainly used for sensing player input (updated every frame)
    private void Update()
    {
        // Read input every rendered frame
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && !jumpRequested && IsGrounded() && !isCrouching && !isSneaking) { jumpRequested = true; }
        if (crouchAction != null && crouchAction.WasPressedThisFrame() && !CrouchRequested && IsGrounded()) { CrouchRequested = true; }
        if (uncrouchAction != null && uncrouchAction.WasPressedThisFrame() && !UncrouchRequested && IsGrounded()) { UncrouchRequested = true; }
        if (sneakAction != null && sneakAction.WasPressedThisFrame() && !SneakRequested && IsGrounded()) { SneakRequested = true; }
    }
    // Mainly used for applying physics movement (updated at fixed time steps)
    private void FixedUpdate()
    {
        // Apply physics movement at fixed time steps
        // Determine current speed based on crouch/sneak state
        curSpeed = (isCrouching || isSneaking) ? crouchSpeed : moveSpeed;

        Vector2 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * curSpeed;
        if (IsGrounded())
        {
            // jump preserves horizontal velocity
            // crouch and uncrouch adjust player collider size and curSpeed to either moveSpeed or sneakSpeed
            HandleCrouch();
            HandleUncrouch();
            HandleSneak();
            if (!isCrouching && !isSneaking) { HandleJump(ref velocity); }
        }
        // Debug.Log($"curSpeed: {curSpeed}, horizontalInput: {horizontalInput}, velocity: {velocity}");
        // Apply the final velocity to the Rigidbody
        rb.linearVelocity = velocity;
    }
    // Logic between Update and FixedUpdate: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Rigidbody-linearVelocity.html 
    // We should confirm with Benno.
}
