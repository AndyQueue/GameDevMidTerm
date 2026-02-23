using UnityEngine;
using System.Collections;

public class LaserButtonVisualsAudio : MonoBehaviour
{
    [Header("Button Animation and Sound")]
    [SerializeField] float animationFPS;
    [SerializeField] Sprite[] spriteAnimation;
    public Color pressedColor;
    public AudioSource buttonSound;

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

    //takes in player to make player go down with button
    private IEnumerator AnimateButton()
    {
        //sets player's parent to button so player goes down w button
        //player.SetParent(this.transform);
        //player's posiiton gets calculated relative to the button
        //we use .transform because that handles position logic

        float waitTime = 1f / animationFPS;

        for (int i = 0; i < spriteAnimation.Length; i++)
        {
            sr.sprite = spriteAnimation[i];
            yield return new WaitForSeconds(waitTime);
        }
        
        //change button color to know it's been pressed
        sr.color = pressedColor;

        //released from button parent
        //player.SetParent(null);
    }
}
