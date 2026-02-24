using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private Vector2 lastMoveDir = Vector2.down;
    public Color originalColor;
    public bool originalColorCaptured;
    public void GetOriginalColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            originalColorCaptured = true;
            Debug.Log("Original color: " + originalColor);
        }
        else
        {
            Debug.LogWarning("PlayerAnimation: SpriteRenderer not found on the GameObject.");
        }
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        Vector2 moveDir = movement.GetMovementDirection();
        bool isMoving = moveDir.sqrMagnitude > 0.05f;
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsGrounded", movement.IsGrounded());
        // animator.SetBool("IsCrouching", movement.IsCrouching());
        animator.SetBool("IsSneaking", movement.IsHiding());

        if (isMoving)
        {
            lastMoveDir = moveDir;
            animator.SetFloat("MoveX", moveDir.x);
            animator.SetFloat("MoveY", moveDir.y);
        }
        else
        {
            animator.SetFloat("MoveX", lastMoveDir.x);
            animator.SetFloat("MoveY", lastMoveDir.y);
            
        }
    }
}