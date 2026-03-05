using UnityEngine;

public class LaserButtonLogic : MonoBehaviour
{
    public GreenLaser laser; //takes in specific laser to turn off 
    public float landingYOffset; //to ensure the player lands on the button instead of running into it

    private bool isPressed = false; //initialize button is not pressed
    private LaserButtonVisualsAudio visualsAndSound; 

    private void Awake()
    {
        visualsAndSound = GetComponent<LaserButtonVisualsAudio>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks to also see if player triggered, if button hasn't been pressed, and if 
        // player is above button - landing on top
        if (other.CompareTag("Player") && !isPressed && other.transform.position.y > transform.position.y + landingYOffset)
        {
            isPressed = true;
            
            //plays button animation
            visualsAndSound.PlayPress(); 
            //calls green laser's turn off laser function
            laser.TurnOffLaser();
        }
    }
}