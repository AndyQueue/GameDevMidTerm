using UnityEngine;

public class LaserButtonLogic : MonoBehaviour
{
    public GreenLaser laser;
    public float landingYOffset; //to ensure the player lands on the button instead of running into it

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
            
            //calls green laser's turn off laser function
            laser.TurnOffLaser();
            //plays button animation
            visualsAndSound.PlayPress();
        }
    }
}