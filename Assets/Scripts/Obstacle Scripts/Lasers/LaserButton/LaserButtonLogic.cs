using UnityEngine;

public class LaserButtonLogic : MonoBehaviour
{
    public GreenLaser laser;
    public float landingYOffset;

    private bool isPressed = false;
    private LaserButtonVisualsAudio visualsAndSound;

    private void Awake()
    {
        visualsAndSound = GetComponent<LaserButtonVisualsAudio>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks to also see if player is above button - landing on top
        if (other.CompareTag("Player") && !isPressed && other.transform.position.y > transform.position.y + landingYOffset)
        {
            isPressed = true;
            
            //checks for safety
            if (laser != null)
            {
                laser.TurnOffLaser();
            }
            if (visualsAndSound != null) 
            {
                visualsAndSound.PlayPress();
            }
        }
    }
}