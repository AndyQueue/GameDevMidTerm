using UnityEngine;

public class CrownScript : MonoBehaviour
{
    //private GameObject player;   
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<PlayerWin>().Won();
            Destroy(gameObject);
        }
    }

}
