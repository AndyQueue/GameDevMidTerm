using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelNumber; //1-based index for level number
    private LevelManager levelManager;


    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogWarning("LevelButton: No LevelManager found in the scene.");
        }

        // TODO: uncomment when not testing!

        // // Disable the button if the previous level is not completed

        // if (levelNumber > 1 && !levelManager.isLevelCompleted(levelNumber - 1))
        // {
        //     // Debug.Log("Previous level " + (levelNumber - 1) + " is not completed. Disabling button for level " + levelNumber);
        //     GetComponentInParent<UnityEngine.UI.Button>().interactable = false;
        // }

    }


    public void OnClick()
    {
        Debug.Log("Level Button for level " + levelNumber + " was Pressed!");
        levelManager.LoadLevel(levelNumber);
    }


}
