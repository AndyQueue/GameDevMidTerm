using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Vector2 moveDir = movement.GetMovementDirection();
        bool isMoving = moveDir.sqrMagnitude > 0.05f;
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsCrouching", movement.IsCrouching());
        animator.SetBool("IsHiding", movement.IsHiding());

        if (isMoving)
        {
            lastMoveDir = moveDir;
            animator.SetFloat("MoveX", moveDir.x);
            animator.SetFloat("MoveY", moveDir.y);

            // Default sprite faces right, so flip only when moving left.
            if (spriteRenderer != null)
            {
                if (moveDir.x < -0.01f) { spriteRenderer.flipX = true; }
                else if (moveDir.x > 0.01f) { spriteRenderer.flipX = false; }
            }
        }
        else
        {
            animator.SetFloat("MoveX", lastMoveDir.x);
            animator.SetFloat("MoveY", lastMoveDir.y);

        }
    }
}