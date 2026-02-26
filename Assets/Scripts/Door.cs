using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameDuration;
    [SerializeField] private AudioSource doorOpenSound;

    [SerializeField] private LevelChanger levelChanger;

    private SpriteRenderer spriteRenderer;

    private bool isOpening = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        yield return new WaitForSeconds(3f); //TODO: fade to black instead of just waiting a few seconds 

    }


    private IEnumerator OpenDoorSequence()
    {
        doorOpenSound.Play();
        yield return StartCoroutine(DoorOpenAnimation());

        if (levelChanger != null)
        {
            levelChanger.LoadNextLevel();
        }
        else
        {
            Debug.LogWarning("Door: LevelChanger reference not set.");
        }
    }

}
