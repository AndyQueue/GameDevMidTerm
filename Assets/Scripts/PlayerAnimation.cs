using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private Rigidbody2D rb;
    private float curDir; // 1 = right, -1 = left
    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 moveDir = movement.GetMovementDirection();
        float horizDir = moveDir.x;
        float verticalVel = rb != null ? rb.linearVelocity.y : 0f;

        // Jumping: use "up" animation
        if (verticalVel > 0.05f)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 1f); // up
            return;
        }

        // Only consider horizontal movement for walk left/right
        bool isMoving = Mathf.Abs(horizDir) > 0.01f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // Use input direction directly: right = +1, left = -1
            curDir = Mathf.Sign(horizDir)+1;
            Debug.Log($"Setting animation direction: {curDir}");
            animator.SetFloat("MoveX", curDir);
            animator.SetFloat("MoveY", 0f);
        }
        else
        {
            // Idle: always face down
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", -1f);
        }
    }
}
