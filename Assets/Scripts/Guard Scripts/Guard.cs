using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
    public AudioSource guardSound;
    public float startOffset; 
    
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private bool hasCaughtPlayer = false;
    private PlayerCaught cachedPlayer; 
    private GuardPatrol guardPatrol;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        guardPatrol = GetComponentInParent<GuardPatrol>();
        Debug.Log("GuardPatrol found: " + (guardPatrol != null));
    }

    void Update()
    {
        if (hasCaughtPlayer) { return; }

        bool byCollision;
        PlayerCaught detected = GetDetectedPlayer(out byCollision);
        if (detected != null)
        {
            cachedPlayer = detected;
            if (byCollision)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            PlayerCaught();
        }
    }

    // Returns the PlayerCaught component if player is detected by either method
    private PlayerCaught GetDetectedPlayer( out bool byCollision)
    {
        Debug.Log("Checking for player detection...");
        byCollision = false;
        // Check collision first (direct contact)
        RaycastHit2D collisionHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size, 0, Vector2.left, 0, playerLayer);

        if (collisionHit.collider != null && collisionHit.collider.CompareTag("Player"))
        {
            PlayerMovement pm = collisionHit.collider.GetComponent<PlayerMovement>();
            if (!pm.IsHiding())
               { 
                    byCollision = true;
                    return collisionHit.collider.GetComponent<PlayerCaught>();
               }
        }

        // Check line of sight
        RaycastHit2D sightHit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0);

        if (sightHit.collider != null && sightHit.collider.CompareTag("Player"))
        {
            PlayerMovement pm = sightHit.collider.GetComponent<PlayerMovement>();
            if (!pm.IsHiding())
                {
                    byCollision = false;
                    return sightHit.collider.GetComponent<PlayerCaught>();
                }
        }

        return null;
    }

    private void PlayerCaught()
    {
        
        hasCaughtPlayer = true;

        if (guardPatrol != null)
        {
            guardPatrol.enabled = false;
            guardPatrol.StopGuard();
        }
        guardSound.time = startOffset;
        guardSound.Play();

        animator.SetBool("IsMoving", false);
        animator.SetTrigger("CatchPlayer");
        StartCoroutine(CatchAfterDelay(0.8f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void CatchPlayer()
    {
        if (cachedPlayer != null)
        {
            OnPlayerDetected(cachedPlayer);
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