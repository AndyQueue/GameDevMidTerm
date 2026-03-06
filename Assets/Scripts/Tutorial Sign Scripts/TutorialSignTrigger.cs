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

    protected abstract bool CheckActionStatus();

    private void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; //invisible at start
        }
    }

    //tutorial signs initiated by trigger with a box collider over area for player to enter
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
    //logic and implementation is understood - explained by own comments to cement understanding 
    private IEnumerator FadeUI(float fadeInOrOut)
    {
        //runs until opacity of canvas group is similar to 1 or 0 (based on parameter) 
        while (!Mathf.Approximately(canvasGroup.alpha, fadeInOrOut))
        {
            // adjusts opacity of the canvas group, MoveTowards uses adds our current opacity to the amount we want 
            // it to change by (fadeSpeed * Time.deltaTime - ensures same fade rate regarless of frame rate) and  
            // ensures that it does not put the return value past the target (fadeInOrOut)
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, fadeInOrOut, fadeSpeed * Time.deltaTime);
            
            yield return null;
            //instructs unity to stop running the opacity adjustment in this frame so that the fade can be 
            // gradual unity will then return in another frame to slightly adjust the opacity again
        }
    }
}
