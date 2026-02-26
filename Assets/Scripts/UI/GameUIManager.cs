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

    private bool isPaused;
    private bool isCaught;

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

    // These are meant to be hooked to Input System actions.
    // Configure a "Pause" action to call OnPause, and a "Reset" (or similar)
    // action to call OnReset in your PlayerInput / input actions setup.

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed || isCaught)
        {
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (!context.performed || !isCaught)
        {
            return;
        }

        ReloadCurrentScene();
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
        // Use unscaled time so it still runs when timeScale = 0
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / flashDuration);
            SetFlashAlpha(t);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(flashHoldDuration);

        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / flashDuration);
            SetFlashAlpha(t);
            yield return null;
        }

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
}
