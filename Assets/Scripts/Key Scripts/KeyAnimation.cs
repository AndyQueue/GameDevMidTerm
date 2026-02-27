using System.Collections;
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
        startPos = transform.position;
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        float animationTimer = 1f / animationFPS;
        int currentFrame = 0;
        while (true) 
        {
            sr.sprite = spriteAnimation[currentFrame];
            currentFrame = (currentFrame + 1) % spriteAnimation.Length;
            yield return new WaitForSeconds(animationTimer);
        }
    }

    void Update()
    {
        //uses sin wave to program a light idle bouncing 
        float newY = startPos.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(startPos.x, newY, 0);
    }
    
}
