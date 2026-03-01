using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameUIManager : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Caught UI")]
    [SerializeField] private GameObject caughtPanel;
    [SerializeField] private Image caughtFlashImage; // full-screen red image
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private float flashHoldDuration = 0.1f;

    public bool isPaused;
    private bool isCaught;
    private PlayerInput playerInput;
    private InputAction pauseAction;
    private InputAction jumpAction;
    private InputAction retryAction;


    private void Awake()
    {
        // Simple setup: assume there is exactly one GameUIManager
        // in the scene, and it does NOT persist across scenes.

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (caughtPanel != null)
        {
            caughtPanel.SetActive(false);
        }

        if (caughtFlashImage != null)
        {
            Color c = caughtFlashImage.color;
            c.a = 0f;
            caughtFlashImage.color = c;
        }
    }

    private void Update()
    {
        // Use the new Input System's Keyboard API directly.
        // This does NOT depend on PlayerInput or Unity Events,
        // so it won't interfere with PlayerMovement.

        var keyboard = Keyboard.current;
        if (keyboard == null) { return; } // No keyboard connected

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null) { return; } // No PlayerInput component
        
        pauseAction = playerInput.actions.FindAction("Pause");
        if (pauseAction == null) { Debug.LogError("Input action 'Pause' not found in PlayerInput actions."); }
        
        jumpAction = playerInput.actions.FindAction("Jump");
        if (jumpAction == null) { Debug.LogError("Input action 'Jump' not found in PlayerInput actions."); }

        retryAction = playerInput.actions.FindAction("Retry");
        if (retryAction == null) { Debug.LogError("Input action 'Retry' not found in PlayerInput actions."); }

        // Pause toggle with Escape
        // Using raw input
        // if (!isCaught && keyboard.escapeKey.wasPressedThisFrame)
        // {
        //     Debug.Log("Escape pressed - toggling pause");
        //     if (isPaused) { ResumeGame(); }
        //     else { PauseGame(); }
        // }

        if (!isCaught && pauseAction.WasPressedThisFrame())
        {
            Debug.Log("Pause action pressed");
            if (isPaused) { ResumeGame(); }
            else { PauseGame(); }
        }
        if (isPaused && jumpAction.WasPressedThisFrame())
        {
            Debug.Log("Jump action pressed while paused - resuming game");
            ResumeGame();
        }

        // When caught, Space reloads the current scene (retry).
        // Using raw input
        // if (isCaught && keyboard.spaceKey.wasPressedThisFrame)
        // {
        //     ReloadCurrentScene();
        // }
        // Using input action
        if (isCaught && retryAction.WasPressedThisFrame())
        {
            Debug.Log("Retry action pressed");
            ReloadCurrentScene();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void BackToMainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1f;
        isPaused = false;
        isCaught = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void HandlePlayerCaught()
    {
        if (isCaught) return;
        isCaught = true;
        // Make sure gameplay is effectively frozen
        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (caughtPanel != null)
        {
            caughtPanel.SetActive(true);
        }

        if (caughtFlashImage != null)
        {
            StartCoroutine(PlayCaughtFlash());
        }
    }

    private IEnumerator PlayCaughtFlash()
    {
        // Repeated blink while the player is caught.
        // Use unscaled time so it still runs when timeScale = 0.
        while (isCaught)
        {
            // Fade in
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / flashDuration);
                SetFlashAlpha(t);
                yield return null;
            }

            // Hold at full opacity
            yield return new WaitForSecondsRealtime(flashHoldDuration);

            // Fade out
            elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = 1f - Mathf.Clamp01(elapsed / flashDuration);
                SetFlashAlpha(t);
                yield return null;
            }

            // Small pause with no red
            SetFlashAlpha(0f);
            yield return new WaitForSecondsRealtime(flashHoldDuration);
        }

        // Ensure we end with no flash if isCaught became false.
        SetFlashAlpha(0f);
    }

    private void SetFlashAlpha(float alpha)
    {
        if (caughtFlashImage == null) return;
        Color c = caughtFlashImage.color;
        c.a = alpha;
        caughtFlashImage.color = c;
    }

    private void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isCaught = false;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // UI button hooks
    public void OnResumeButton()
    {
        ResumeGame();
    }

    public void OnBackButton(string mainMenuSceneName)
    {
        BackToMainMenu(mainMenuSceneName);
    }

    public void OnRestartButton()
    {
        ReloadCurrentScene();
    }
}
