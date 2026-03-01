using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource collectKeySound;
    public float startOffset; //for audio skip
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //updates player's inventory to mark that the player has a key
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            
            //skips slightly into audio and plays key collection sound
            collectKeySound.time = startOffset; //sets the time to play from to the start offset
            collectKeySound.Play();

            //marks that player has key for door logic
            inventory.hasKey = true;
            Destroy(gameObject);
            //destroys key when player collects key
        }
    }

}
