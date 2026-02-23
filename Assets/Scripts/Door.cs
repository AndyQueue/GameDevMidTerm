using UnityEngine;

public class Door : MonoBehaviour
{
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
        Destroy(gameObject);
        //trigger animation of opening door 
    }
}
