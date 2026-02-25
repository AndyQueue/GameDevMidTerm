using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (PlayerInSight())
        {
            Debug.Log("Player Spotted!");
            animator.SetTrigger("CatchPlayer");
            CatchPlayer();
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
    {
        //implement catch player
    }
}
