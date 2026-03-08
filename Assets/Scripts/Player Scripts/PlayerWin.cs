using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWin : MonoBehaviour
{
    public bool hasWon = false;

    private GameUIManager gameUIManager;
    public void Awake()
    {
        gameUIManager = FindFirstObjectByType<GameUIManager>();
    }
    public void Won()
    {
        if (hasWon) return;

        hasWon = true;

        Debug.Log("Player Wins");
        // Use the UI manager reference set in the Inspector.
        if (gameUIManager != null)
        {
            gameUIManager.HandlePlayerWin();
        }
        else
        {
            // Fallback: reload scene immediately if no UI manager is assigned.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
