using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private LevelChanger levelChanger;

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
        Debug.Log("Door Opened! Yay you win!");

        if (levelChanger != null)
        {
            levelChanger.LoadNextLevel();
        }
        else
        {
            Debug.LogWarning("Door: LevelChanger reference not set, cannot load next level.");
        }

        Destroy(gameObject);
        //trigger animation of opening door 
    }
}
