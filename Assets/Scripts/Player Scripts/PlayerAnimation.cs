using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Player SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip crouchSFX;
    [SerializeField] private AudioClip uncrouchSFX;
    [SerializeField] private AudioClip hideSFX;
    [SerializeField] private AudioClip unhideSFX;

    private Animator animator;
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastMoveDir = Vector2.down;
    public Color originalColor;
    public bool originalColorCaptured;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("Name of audio source for jump SFX: " + (jumpSFX != null ? jumpSFX.name : "None"));
        Debug.Log("Name of audio source for crouch SFX: " + (crouchSFX != null ? crouchSFX.name : "None"));
        Debug.Log("Name of audio source for uncrouch SFX: " + (uncrouchSFX != null ? uncrouchSFX.name : "None"));
        Debug.Log("Name of audio source for hide SFX: " + (hideSFX != null ? hideSFX.name : "None"));
        Debug.Log("Name of audio source for unhide SFX: " + (unhideSFX != null ? unhideSFX.name : "None"));
    }

    public void PlaySfx(string clip)
    {
        if (sfxSource == null)
        {
            Debug.LogWarning("AudioSource is missing. Cannot play sound.");
            return;
        }
        string clipName = clip + "SFX";
        AudioClip audioClip = null;
        switch (clipName)
        {
            case "JumpSFX":
                audioClip = jumpSFX;
                break;
            case "CrouchSFX":
                audioClip = crouchSFX;
                break;
            case "UncrouchSFX":
                audioClip = uncrouchSFX;
                break;
            case "HideSFX":
                audioClip = hideSFX;
                break;
            case "UnhideSFX":
                audioClip = unhideSFX;
                break;
        }
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip not found for name: " + clipName);
            return;
        }
        sfxSource.PlayOneShot(audioClip);
    }

    private void Update()
    {
        Vector2 moveDir = movement.GetMovementDirection();
        bool isMoving = moveDir.sqrMagnitude > 0.05f;
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsCrouching", movement.IsCrouching());

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