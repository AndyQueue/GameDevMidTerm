using UnityEngine;

public class GreenLaser : Laser
{
    [Header("Audio")]
    public AudioSource turnOffLaserSound;

    void Start()
    {
        SetLaserState(true);
    }

    //has a turn off laser function for when the button is pressed
    public void TurnOffLaser()
    {
        turnOffLaserSound.Play();
        SetLaserState(false);
    }
}
