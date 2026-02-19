using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OpenDoor(){
        Destroy(gameObject);
        //trigger animation of opening door 
    }
}
