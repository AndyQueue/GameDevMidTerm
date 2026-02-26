using UnityEngine;
using System.Collections;
using TMPro;

public abstract class TutorialSignTrigger : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvasGroup;
    //use a canvas group here to fade all ui rather than just text
    public float fadeSpeed;
    
    protected bool playerInTriggerArea = false;
    protected bool actionCompleted = false;

    //children define how the tutorial action is completed
    protected abstract bool CheckActionStatus();

    private void Start()
    {
        if (canvasGroup != null) canvasGroup.alpha = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !actionCompleted)
        {
            playerInTriggerArea = true;
            StopAllCoroutines();
            //stop all coroutines to avoid bugs when going in and out of trigger area
            StartCoroutine(FadeUI(1f));
        }
    }

    private void Update()
    {
        if (playerInTriggerArea && !actionCompleted)
        {
            if (CheckActionStatus())
            {
                actionCompleted = true;
                playerInTriggerArea = false;
                StopAllCoroutines();
                StartCoroutine(FadeUI(0f));
            }
        }
    }

    //obtained FadeUI logic with float to allow fade in and out with same function from google Gemini
    //prompt: how do I fade UI elements smoothly in unity - gemini suggested use of canvasGroup to make
    //the whole process cleaner and smoother
    private IEnumerator FadeUI(float fadeInOrOut)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, fadeInOrOut))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, fadeInOrOut, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
