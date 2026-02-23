using UnityEngine;
using System.Collections;

public class LaserButtonVisualsAudio : MonoBehaviour
{
    [Header("Button Animation and Sound")]
    [SerializeField] float animationFPS;
    [SerializeField] Sprite[] spriteAnimation;
    public Color pressedColor;
    public AudioSource buttonSound;
    
    private PolygonCollider2D polyCol;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void PlayPress()
    {
        buttonSound.Play();
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        float waitTime = 1f / animationFPS;
        BoxCollider2D col = GetComponent<BoxCollider2D>();

        for (int i = 0; i < spriteAnimation.Length; i++)
        {
            sr.sprite = spriteAnimation[i];
            col.size = sr.sprite.bounds.size;
            col.offset = sr.sprite.bounds.center;
            yield return new WaitForSeconds(waitTime);
        }
        
        //change button color to know it's been pressed
        sr.color = pressedColor;
    }
}
