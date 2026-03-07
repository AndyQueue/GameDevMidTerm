using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameDuration = 0.5f;
    [SerializeField] private AudioSource doorOpenSound;
    [SerializeField] private AudioSource doorCreakSound;
    [SerializeField] private AudioSource doorEnterSound;
    [SerializeField] private AudioSource doorLockedSound;


    private LevelManager levelManager;

    private SpriteRenderer spriteRenderer;
    private GameObject player;

    private bool isOpening = false;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogWarning("Door: No LevelManager found in the scene.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
        //ensures we are colliding with player by checking that inventory exists
        if (inventory != null)
        {
            //checks inventory for key to unlock door
            if (inventory.hasKey)
            {
                // Debug.Log("has key, opening door");
                OpenDoor();
            }
            else
            {
                doorLockedSound.Play();
                Debug.Log("You don't have the key, door is locked.");
            }
        }
    }

    private void OpenDoor()
    {
        if (isOpening) return;
        isOpening = true;
        GetComponent<BoxCollider2D>().enabled = false; //disable collider to allow player to walk into the door
        int currentLevel = levelManager.GetCurrentLevelNumber();
        levelManager.completeLevel(currentLevel); // mark the level as completed
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
        // yield return new WaitForSeconds(0f); //TODO: fade to black instead of just waiting a few seconds 

    }

    private IEnumerator OpenDoorSequence()
    {
        doorOpenSound.Play();
        yield return new WaitForSeconds(0.2f);
        doorCreakSound.Play();

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        PlayerInput input = player.GetComponent<PlayerInput>();

        input.DeactivateInput(); // disable player input during the door opening sequence
        yield return StartCoroutine(PlayerJumpAnimation(0.4f, 0f, 0.1f));

        movement.MoveToPosition(transform.position); // walk into the door
        yield return StartCoroutine(DoorOpenAnimation());

        yield return new WaitUntil(() => !movement.IsAutoMoving());

        doorEnterSound.Play();

        input.ActivateInput(); // re-enable player input after the door sequence

        player.GetComponent<SpriteRenderer>().enabled = false; // make player dissapear after door opens

        yield return new WaitForSeconds(1f); // wait a moment before loading the next level

        levelManager.LoadNextLevel();
    }

    private IEnumerator PlayerJumpAnimation(float jumpHeight, float jumpEndHeight, float jumpDuration)
    {
        // This animation was written using AI (Claude)

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
