// this script is mainly from a Youtube tutorial, edited to fit our game. https://youtu.be/d002CljR-KU?si=gM51dETCUJaY7Swd
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
        guardPatrol = GetComponentInParent<GuardPatrol>(); // Try to find GuardPatrol in parent objects
    }

    void Update()
    {   
        // if player is already caught, skip detection logic
        if (hasCaughtPlayer) { return; }

        //AI helped writing
        bool byCollision;
        PlayerCaught detected = GetDetectedPlayer(out byCollision); //get the player detected
        if (detected != null)
        {
            cachedPlayer = detected;
            // flip guard to face player if detected by collision
            if (byCollision)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            PlayerCaught();
        }
    }

    // AI helped writing this method
    // Returns the PlayerCaught component if player is detected by either method
    private PlayerCaught GetDetectedPlayer( out bool byCollision)
    {
        byCollision = false;
        // Check collision first (direct contact)
        RaycastHit2D collisionHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size, 0, Vector2.left, 0);

        if (collisionHit.collider != null && collisionHit.collider.CompareTag("Player"))
        {
            Debug.Log("Colliding");
            PlayerMovement pm = collisionHit.collider.GetComponent<PlayerMovement>();
            //return the player that was caught if not hidden
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
            //return the player that was caught if not hidden
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

        // disable and stop the guard from moving
        if (guardPatrol != null)
        {
            guardPatrol.enabled = false;
            guardPatrol.StopGuard();
        }
        // guard sfx "hey!"
        guardSound.time = startOffset;
        // skips ahead to correct start point in audio
        guardSound.Play();
        // guard animation catching player
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("CatchPlayer");
        StartCoroutine(CatchAfterDelay(0.8f));
    }

    //visualize view range of guard in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    // AI helped writing this method
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

    //coroutine to allow animation to play before catching player
    private IEnumerator CatchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CatchPlayer();
    }
}