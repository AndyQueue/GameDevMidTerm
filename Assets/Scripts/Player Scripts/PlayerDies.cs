using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDies : MonoBehaviour
{
    public bool isDead = false;

    // Drag the GameUIManager from the scene into this field in the Inspector.
    [SerializeField] private GameUIManager gameUIManager;

    public void Dies()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player Died");

        // Use the UI manager reference set in the Inspector.
        if (gameUIManager != null)
        {
            gameUIManager.HandlePlayerCaught();
        }
        else
        {
            // Fallback: reload scene immediately if no UI manager is assigned.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
