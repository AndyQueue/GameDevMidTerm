using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class KeyAnimation : MonoBehaviour
{
    
    [Header("Sparkle Animation")]
    [SerializeField] float animationFPS;
    [SerializeField] Sprite[] spriteAnimation;

    [Header("Bounce Settings")]
    [SerializeField] float bounceHeight; 
    [SerializeField] float bounceSpeed;

    private SpriteRenderer sr;
    private Vector3 startPos;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position; //gets current position of key to program idle bouncing animation
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        float animationTimer = 1f / animationFPS; //calculates time each animation stays on the screen
        //time per frame = 1/ frames per second
        int currentFrame = 0; 
        while (true) 
        {
            sr.sprite = spriteAnimation[currentFrame]; //sets the sprite to the current frame in the animation
            currentFrame = (currentFrame + 1) % spriteAnimation.Length; //gets the new current frame by going 
            // to the next one and modding by the length of the sprite animations to ensure it plays in a loop
            // and does not stop after the last sprite in the array
            yield return new WaitForSeconds(animationTimer); //haults for time for the current sprite to be displayed
        }
    }

    void Update()
    {
        //uses sin wave to program a light idle bouncing 
        //obtained math calculation from google gemini
        //prompt: how do I program a light idle bouncing animation taking into account bouncing speed and 
        //height in unity
        float newY = startPos.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(startPos.x, newY, 0);
    }
    
}
