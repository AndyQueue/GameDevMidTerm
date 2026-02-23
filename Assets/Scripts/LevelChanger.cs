using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    public void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("LevelChanger: nextSceneName is not set.");
            return;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}
