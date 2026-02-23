using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction duckAction;
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool jumpRequested;
    private bool duckRequested;

    private void Awake()
    {
        // disable all actions at the start
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        { Debug.LogError("PlayerInput component is missing on PlayerMovement GameObject."); return; }

        jumpAction = playerInput.actions.FindAction("Jump");
        duckAction = playerInput.actions.FindAction("Crouch");

        if (jumpAction == null) { Debug.LogError("Input action 'Jump' not found in PlayerInput actions."); }

        if (duckAction == null) { Debug.LogError("Input action 'Crouch' not found in PlayerInput actions."); }
    }

    public void OnMove(InputValue input)
    {
        // Only use horizontal input for movement; up/down reserved for jump/duck
        Vector2 move = input.Get<Vector2>();
        horizontalInput = move.x;

    }
    public Vector2 GetMovementDirection()
    {
        return new Vector2(horizontalInput, 0f);
    }
    private bool IsGrounded()
    {
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider != null ? Color.green : Color.red);
        return hit.collider != null;
    }
    private void HandleJump(ref Vector2 velocity)
    {
        if (!jumpRequested) { return; }
        Debug.Log("Jump pressed");
        if (IsGrounded())
        {
            Debug.Log("is grounded");
            jumpRequested = false; // consume the jump
            velocity.y = jumpForce;
        }
    }
    private void HandleDuck()
    {
        if (!duckRequested) { return; }
        Debug.Log("Duck pressed");
        if (IsGrounded())
        {
            duckRequested = false; // consume the duck
            // Duck here
        }
    }
    
// Mainly used for sensing player input (updated every frame)
    private void Update()
    {
        // Read input every rendered frame
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && !jumpRequested && IsGrounded()) { jumpRequested = true; }
        if (duckAction != null && duckAction.WasPressedThisFrame() && !duckRequested && IsGrounded()) { duckRequested = true; }
    }
// Mainly used for applying physics movement (updated at fixed time steps)
    private void FixedUpdate()
    {
        // Apply physics movement at fixed time steps
        Vector2 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * moveSpeed;

        // jump preserves horizontal velocity
        HandleJump(ref velocity);
        // doesn't do anything yet
        HandleDuck();

        // Apply the final velocity to the Rigidbody
        rb.linearVelocity = velocity;
    }
// Logic between Update and FixedUpdate: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Rigidbody-linearVelocity.html 
// We should confirm with Benno.
}
