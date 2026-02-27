using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentLevelNumber; //1-based index for current level number


    public void InitializeLevelData()
    {
        for (int i = 0; i < LevelData.levelData.Length; i++)
        {
            LevelData.levelData[i] = false;
        }
    }

    public void completeLevel(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > LevelData.levelData.Length)
        {
            Debug.LogWarning("LevelManager: Invalid level number " + levelNumber);
            return;
        }
        LevelData.levelData[levelNumber - 1] = true;
        Debug.Log("Level " + levelNumber + " is completed!");
    }

    public bool isLevelCompleted(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > LevelData.levelData.Length)
        {
            Debug.LogWarning("LevelManager: Invalid level number " + levelNumber);
            return false;
        }

        return LevelData.levelData[levelNumber - 1];
    }

    public int GetCurrentLevelNumber()
    {
        return currentLevelNumber;
    }

    public void LoadNextLevel()
    {
        string nextSceneName = "Level " + (currentLevelNumber + 1);
        Debug.Log("Loading next level: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    public void LoadLevel(int levelNumber)
    {
        string sceneName = "Level " + levelNumber;
        Debug.Log("Loading level: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }


}