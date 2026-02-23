using UnityEngine;

public class Sneaking : StateMachineBehaviour
{
    private Color originalColor;
    private bool originalColorCaptured = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SpriteRenderer spriteRenderer = animator.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Preserve the original color
            if (!originalColorCaptured)
            {
                originalColor = spriteRenderer.color;
                originalColorCaptured = true;
            }
            // Gray out player sprite to indicate sneak state
            spriteRenderer.color = Color.gray;
        }
        else
        {
            Debug.LogWarning("Sneaking: SpriteRenderer not found on the Animator's GameObject.");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SpriteRenderer spriteRenderer = animator.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Ensure the sprite remains gray while in the sneak state
            spriteRenderer.color = Color.gray;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SpriteRenderer spriteRenderer = animator.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && originalColorCaptured)
        {
            spriteRenderer.color = originalColor;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
