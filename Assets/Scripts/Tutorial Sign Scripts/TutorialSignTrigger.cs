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
        if (canvasGroup != null) 
        {
            canvasGroup.alpha = 0; //makes canvas group invisible at the start
        }
    }

    //tutorial signs initiated by "is trigger" box collider 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !actionCompleted)
        {
            playerInTriggerArea = true;
            //becomes true so even if the player moves the sign stays until desired action is complete
            StopAllCoroutines();
            //stop all coroutines here as a safety check in case player enters in 2 frames and calls 
            //fade function multiple times
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
                //change booleans so sign does not reappear 

                StopAllCoroutines();
                //stop all coroutines to avoid fading in UI and fading out UI to conflict
                //since we use a coroutine it runs in the background and will keep running when we want to 
                //fade out the UI since it runs with a while loop

                StartCoroutine(FadeUI(0f));
            }
        }
    }

    //obtained smooth FadeUI logic with float timer to allow fade in and out with same function from google 
    //Gemini 
    //prompt: how do I fade UI elements smoothly in unity, pasted current script with all functions but fadeUI
    // gemini generated fadeUI function and suggested use of canvasGroup to make the process cleaner 
    // and smoother, also instructed the use of StopAllCoroutines to ensure we are not calling conflicting fading
    // functions at the same time 
    private IEnumerator FadeUI(float fadeInOrOut)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, fadeInOrOut))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, fadeInOrOut, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
