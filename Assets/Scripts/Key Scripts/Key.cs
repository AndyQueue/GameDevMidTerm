using UnityEngine;

public class Key : MonoBehaviour
{
    // public AudioSource collectKeySound;
    public float startOffset;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                // collectKeySound.time = startOffset;
                // collectKeySound.Play();
                inventory.hasKey = true;
                Destroy(gameObject);
            }
        }
    }

}
