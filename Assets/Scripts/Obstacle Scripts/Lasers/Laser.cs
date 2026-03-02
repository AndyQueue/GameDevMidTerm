using UnityEngine;

public abstract class Laser : MonoBehaviour, IDetector
{
    protected SpriteRenderer spriteRenderer;
    protected Collider2D laserCollider;
    protected bool isLaserActive = true;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        laserCollider = GetComponent<Collider2D>();
    }

    // sets the laser state to active or inactive based on boolean, used to intitialize on 
    // state of all lasers and to turn blue laser on and off, and green laser off after hitting button
    protected virtual void SetLaserState(bool active)
    {
        isLaserActive = active;
        spriteRenderer.enabled = active;
        laserCollider.enabled = active;
    }

    public void OnPlayerDetected(PlayerCaught player)
    {
        if (isLaserActive)
        {
            player?.Caught();
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerMovement>().IsHiding()) { return; }
            PlayerCaught player = other.GetComponent<PlayerCaught>();
            OnPlayerDetected(player);
        }
    }
}
