using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private bool hasCaughtPlayer = false;

    private Animator animator;

    private GuardPatrol guardPatrol;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        guardPatrol = GetComponentInParent<GuardPatrol>();
        Debug.Log("GuardPatrol found: " + (guardPatrol != null));
    }
    void Update()
    {
        if (hasCaughtPlayer) { return; }
        bool playerDetected = PlayerInSight();
        if (playerDetected)
        {
            hasCaughtPlayer = true;

            if (guardPatrol != null)
            {
                guardPatrol.enabled = false;
                guardPatrol.StopGuard();
            }

            animator.SetBool("IsMoving", false);
            animator.SetTrigger("CatchPlayer");
            //CatchPlayer();
            StartCoroutine(CatchAfterDelay(0.8f));

        } 
        
    }

    private bool PlayerInSight()
    {
        // using player layer mask
        // RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range, boxCollider.bounds.size, 0, Vector2.left, 0, playerLayer);
        // return hit.collider != null;

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (hit.collider.GetComponent<PlayerMovement>().IsHiding()) { return false; }
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void CatchPlayer()
    {        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0);

            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.GetComponent<PlayerMovement>().IsHiding()) { return; }
                PlayerCaught player = hit.collider.GetComponent<PlayerCaught>();
                OnPlayerDetected(player);
            }
    
    }

    public void OnPlayerDetected(PlayerCaught player)
    {
            player?.Caught();
    }

        private IEnumerator CatchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CatchPlayer();
    }
}
