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

    //called by other button script to animate button press
    public void PlayPress()
    {
        buttonSound.Play();
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        float waitTime = 1f / animationFPS; //get seconds per frame - with 1/(frames/second)
        BoxCollider2D col = GetComponent<BoxCollider2D>(); 
        //get collider of button to adjust based on new sprite renderer

        for (int i = 0; i < spriteAnimation.Length; i++) //loop only runs until the button is down
        {
            sr.sprite = spriteAnimation[i]; 
        
            //adjust collider size and pivot point
            col.size = sr.sprite.bounds.size;
            col.offset = sr.sprite.bounds.center;

            yield return new WaitForSeconds(waitTime);
        }
        
    
    }
}
