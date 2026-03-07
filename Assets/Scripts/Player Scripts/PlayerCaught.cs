using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCaught : MonoBehaviour
{
    public bool isCaught = false;

    private GameUIManager gameUIManager;
    public void Awake()
    {
        gameUIManager = FindFirstObjectByType<GameUIManager>();
    }
    public void Caught()
    {
        if (isCaught) return;

        isCaught = true;

        Debug.Log("Player Caught");
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
