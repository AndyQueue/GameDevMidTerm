using UnityEngine;

public class CrownScript : MonoBehaviour
{

    public AudioSource collectCrownSound;
    public float startOffset; //for audio skip

        private GameUIManager gameUIManager;

    public void Awake()
    {
        gameUIManager = FindFirstObjectByType<GameUIManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //get player's inventory to update hasKey
            // PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            
            //skips slightly into audio and plays crown collection sound
            collectCrownSound.time = startOffset; 
            collectCrownSound.Play();

            //marks that player has crown
            // inventory.hasCrown = true;
            Destroy(gameObject);

        }
    }

}
