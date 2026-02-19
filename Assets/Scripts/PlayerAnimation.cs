// using UnityEngine;
// [RequireComponent(typeof(Animator))]
// public class PlayerAnimation : MonoBehaviour
// {
//     private Animator animator;
//     private PlayerMovement movement;
//     private Rigidbody2D rb;
//     private float curDir; // 1 = right, -1 = left
//     private void Awake()
//     {
//         animator = GetComponent<Animator>();
//         movement = GetComponent<PlayerMovement>();
//         rb = GetComponent<Rigidbody2D>();
//     }

//     private void Update()
//     {
//         Vector2 moveDir = movement.GetMovementDirection();
//         Debug.Log($"Move direction: {moveDir}");
//         float horizDir = moveDir.x;
//         float verticalVel = rb != null ? rb.linearVelocity.y : 0f;

//         // // Jumping: use "up" animation
//         // if (verticalVel > 0.05f)
//         // {
//         //     animator.SetBool("IsMoving", true);
//         //     animator.SetFloat("MoveX", 0f);
//         //     animator.SetFloat("MoveY", 1f); // up
//         //     return;
//         // }

//         // // Only consider horizontal movement for walk left/right
//         // bool isMoving = Mathf.Abs(horizDir) > 0.01f;
//         bool isMoving = moveDir.sqrMagnitude > 0.01f;
//         // animator.SetBool("IsMoving", isMoving);

//         if (isMoving)
//         {
//             // Use input direction directly: right = +1, left = -1
//             curDir = Mathf.Sign(horizDir);
//             Debug.Log($"Horizontal input: {horizDir}, setting direction to: {curDir}");
//             // Debug.Log($"Setting animation direction: {curDir}");
//             animator.SetFloat("MoveX", horizDir);
//             animator.SetFloat("MoveY", verticalVel);
//         }
//         // else
//         // {
//         //     // Idle: always face down
//         //     animator.SetFloat("MoveX", 0f);
//         //     animator.SetFloat("MoveY", -1f);
//         // }
//     }
// }

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    private PlayerMovement movement;
    private Vector2 lastMoveDir = Vector2.down;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Vector2 moveDir = movement.GetMovementDirection();
        bool isMoving = moveDir.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);
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