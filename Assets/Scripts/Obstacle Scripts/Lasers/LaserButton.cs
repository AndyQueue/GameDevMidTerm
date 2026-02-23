using System.Collections;
using UnityEngine;

public class LaserButton : MonoBehaviour
{
    public GreenLaser laser;
    
    [Header("Button Animation")]
    [SerializeField] float animationFPS;
    [SerializeField] Sprite[] spriteAnimation;
    public Color pressedColor;

    [Header("Button Landing")]
    public float landingYOffset;
    public AudioSource buttonSound;



    private SpriteRenderer sr;
    private bool isPressed = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks to also see if player is above button - landing on top
        if (other.CompareTag("Player") && !isPressed && other.transform.position.y > transform.position.y + landingYOffset)
        {
            isPressed = true;
            
            if (laser != null)
            {
                laser.TurnOffLaser();
            }

            buttonSound.Play();
            StartCoroutine(AnimateButton());
        }
    }

    private IEnumerator AnimateButton()
    {
        float waitTime = 1f / animationFPS;

        for (int i = 0; i < spriteAnimation.Length; i++)
        {
            sr.sprite = spriteAnimation[i];
            yield return new WaitForSeconds(waitTime);
        }
        
        //change button color to know it's been pressed
        sr.color = pressedColor;
    }
}