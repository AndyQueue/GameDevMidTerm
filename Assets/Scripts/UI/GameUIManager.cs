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

        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            pauseAction = playerInput.actions.FindAction("Pause");
            retryAction = playerInput.actions.FindAction("Retry");
        }
    }

    private void Update()
    {
        if (playerInput == null)
        {
            return;
        }

        // Pause toggle with Escape
        if (!isCaught && pauseAction != null && pauseAction.WasPressedThisFrame())
        {
            Debug.Log("Pause action pressed");
            if (isPaused) { ResumeGame(); }
            else { PauseGame(); }
        }
        // Resume with Jump when paused
        if (isPaused && retryAction != null && retryAction.WasPressedThisFrame())
        {
            Debug.Log("Retry action pressed while paused - resuming game");
            ResumeGame();
        }
        // Retry with Space when caught
        if (isCaught && retryAction != null && retryAction.WasPressedThisFrame())
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
        else
        {
            Debug.LogWarning("GameUIManager: Pause panel reference is missing.");
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isCaught = false;
        SceneManager.LoadScene("MainMenu");
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
        // Uses unscaled time so it still runs when timeScale = 0.
        while (isCaught)
        {
            // Fade in
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                // t goes from 0 to 1 over the duration of the flash
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

            // Hold at no opacity
            SetFlashAlpha(0f);
            yield return new WaitForSecondsRealtime(flashHoldDuration);
        }
        SetFlashAlpha(0f);
    }

    private void SetFlashAlpha(float alpha)
    {
        if (caughtFlashImage == null) return;
        Color caughtColor = caughtFlashImage.color;
        caughtColor.a = alpha;
        caughtFlashImage.color = caughtColor;
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
        Debug.Log("Resume button clicked");
        ResumeGame();
    }

    public void OnBackButton()
    {
        Debug.Log("Back button clicked");
        BackToMainMenu();
    }

    public void OnRestartButton()
    {
        ReloadCurrentScene();
    }
}
