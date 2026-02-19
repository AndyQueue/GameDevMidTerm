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
    private Vector2 movement;

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
        Vector2 inputVec = input.Get<Vector2>();
        movement = new Vector2(inputVec.x, 0f);
    }

    public Vector2 GetMovementDirection()
    {
        return movement;
    }

    private bool IsGrounded()
    {
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
#if UNITY_EDITOR
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider != null ? Color.green : Color.red);
#endif
        return hit.collider != null;
    }

    private void HandleJump(ref Vector2 velocity)
    {
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Debug.Log("Jump pressed");
            velocity.y = jumpForce;
        }
    }

    private void HandleDuck(ref Vector2 velocity)
    {
        if (duckAction != null && duckAction.WasPressedThisFrame())
        {
            Debug.Log("Duck pressed");
            velocity.y = -jumpForce;
        }
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;

        // apply left/right movement
        Vector2 moveDir = GetMovementDirection();
        velocity.x = moveDir.x * moveSpeed;

        // face left/right when moving horizontally
        if (Mathf.Abs(velocity.x) > 0.001f)
        {
            transform.right = new Vector2(Mathf.Sign(velocity.x), 0f);
        }

        // handle jump (up key) and duck (down key)
        HandleJump(ref velocity);
        HandleDuck(ref velocity);

        rb.linearVelocity = velocity;
    }
}
