using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameDuration = 0.35f;
    [SerializeField] private AudioSource doorCreakSound;
    [SerializeField] private AudioSource doorEnterSound;

    [SerializeField] private LevelChanger levelChanger;

    private SpriteRenderer spriteRenderer;
    private GameObject player;

    private bool isOpening = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            if (inventory.hasKey)
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        if (isOpening) return;
        isOpening = true;
        Debug.Log("Door Opened! Yay you win!");

        StartCoroutine(OpenDoorSequence());
    }

    private IEnumerator DoorOpenAnimation()
    {
        if (frames == null || frames.Length == 0)
        {
            Debug.LogWarning("DoorOpenAnimation: No animation frames set.");
            yield break;
        }

        for (int i = 0; i < frames.Length; i++)
        {
            spriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(frameDuration);
        }
        yield return new WaitForSeconds(0f); //TODO: fade to black instead of just waiting a few seconds 

    }

    private IEnumerator OpenDoorSequence()
    {
        doorCreakSound.Play();
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        PlayerInput input = player.GetComponent<PlayerInput>();

        input.DeactivateInput(); // disable player input during the door opening sequence
        yield return StartCoroutine(PlayerJumpAnimation(0.4f, 0f, 0.1f));

        movement.MoveToPosition(transform.position); // walk to door position
        yield return StartCoroutine(DoorOpenAnimation());

        yield return new WaitUntil(() => !movement.IsAutoMoving());

        doorEnterSound.Play();

        yield return StartCoroutine(PlayerJumpAnimation(0.8f, -5f, 0.3f));

        input.ActivateInput(); // re-enable player input after the door sequence

        if (levelChanger != null)
        {
            levelChanger.LoadNextLevel();
        }
        else
        {
            Debug.LogWarning("Door: LevelChanger reference not set.");
        }
    }

    private IEnumerator PlayerJumpAnimation(float jumpHeight, float jumpEndHeight, float jumpDuration)
    {
        Vector3 startPos = player.transform.position;
        Vector3 peakPos = startPos + new Vector3(0, jumpHeight, 0);
        Vector3 bottomPos = startPos + new Vector3(0, jumpEndHeight, 0); // end height so player can dissapear "under" the door

        // Go up
        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            player.transform.position = Vector3.Lerp(startPos, peakPos, elapsed / jumpDuration);
            yield return null;
        }

        // Come down
        elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            player.transform.position = Vector3.Lerp(peakPos, bottomPos, elapsed / jumpDuration);
            yield return null;
        }

        player.transform.position = bottomPos; // snap to exact position


    }



}
