using UnityEngine;
using System.Collections;

public class LaserButtonVisualsAudio : MonoBehaviour
{
    [Header("Button Animation and Sound")]
    [SerializeField] float animationFPS;
    [SerializeField] Sprite[] spriteAnimation;

    public AudioSource buttonSound;
    
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    //function called by other button script to animate button press
    public void PlayPress()
    {
        buttonSound.Play();
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        float waitTime = 1f / animationFPS; //get seconds per frame with 1/ frames per second
        BoxCollider2D col = GetComponent<BoxCollider2D>(); 
        //get collider of button to adjust based on new sprite renderer

        for (int i = 0; i < spriteAnimation.Length; i++) //loop only runs until the button is down
        {
            sr.sprite = spriteAnimation[i];
            
            // resets the box collider size of the button to adjust for the new sprite renderer so the player 
            // goes down with the button as it is being pressed
            col.size = sr.sprite.bounds.size;
            
            // sets the box collider's center so that the collider is in the right position 
            //  it was the right size but will just be shifted upwards until we adjust the pivot point 
            col.offset = sr.sprite.bounds.center;

            yield return new WaitForSeconds(waitTime);
        }
    
    }
}
