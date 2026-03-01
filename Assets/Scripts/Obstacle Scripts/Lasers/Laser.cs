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

    protected virtual void SetLaserState(bool active)
    {
        isLaserActive = active;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = active;
        }
        if (laserCollider != null)
        {
            laserCollider.enabled = active;
        }
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
