using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource collectKeySound;
    public float startOffset; //for audio skip
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //get player's inventory to update hasKey
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            
            //skips slightly into audio and plays key collection sound
            collectKeySound.time = startOffset; 
            collectKeySound.Play();

            //marks that player has key
            inventory.hasKey = true;
            Destroy(gameObject);
        }
    }

}
