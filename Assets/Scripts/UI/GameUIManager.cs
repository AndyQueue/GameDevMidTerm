using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Caught UI")]
    [SerializeField] private GameObject caughtPanel;
    [SerializeField] private Image caughtFlashImage;        // flashing red image
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private float flashHoldDuration = 0.1f;
    [SerializeField] private AudioSource caughtSFX;

    [Header("Win UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private AudioSource winSFX;

    private bool isCaught;
    private bool hasWon;

    private void Awake()
    {
        GameState.SetPaused(false);

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

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void OnPause()
    {
        if (isCaught) { return; }

        if (!GameState.IsGamePaused()) { PauseGame(); }
        else if (GameState.IsGamePaused()) { ResumeGame(); }
        else { Debug.LogWarning("Pause Failed"); }
    }
    public void OnRetry()
    {
        // Only works if player is caught, otherwise space should only register as jump
        if (isCaught) { ReloadCurrentScene(); }
        if (GameState.IsGamePaused()) { ResumeGame(); }
    }

    // Linked with back button's "On Click" since no keyboard key linked
    public void OnBackButton()
    {
        Debug.Log("Back button clicked");
        BackToMainMenu();
    }
    public void PauseGame()
    {
        GameState.SetPaused(true);
        Time.timeScale = 0f;
        if (pausePanel != null) { pausePanel.SetActive(true); }
    }

    public void ResumeGame()
    {
        GameState.SetPaused(false);
        Time.timeScale = 1f;
        if (pausePanel != null) { pausePanel.SetActive(false); }
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        GameState.SetPaused(false);
        isCaught = false;
        SceneManager.LoadScene("MainMenu");
    }
    public void HandlePlayerCaught()
    {
        if (isCaught) return;
        isCaught = true;
        GameState.SetPaused(false);
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
        if (caughtSFX != null)
        {
            caughtSFX.Play();
        }
    }

    // Repeated blink while the player is caught.
    private IEnumerator PlayCaughtFlash()
    {
        while (isCaught)
        {
            // Fade in
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                // Since timeScale = 0, use unscaled time.
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

       public void HandlePlayerWin()
    {
        if (hasWon) return;
        hasWon = true;
        GameState.SetPaused(false);
        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        if (winSFX != null)
        {
            if (CrownRoomMusicManager.instance != null)
                CrownRoomMusicManager.instance.bgMusic.Pause();
            winSFX.Play();
        }
    }

    private void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        GameState.SetPaused(false);
        isCaught = false;
        hasWon = false;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
}
