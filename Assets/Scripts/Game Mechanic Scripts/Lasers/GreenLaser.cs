using UnityEngine;
using System.Collections;

public class GreenLaser : Laser
{
    [Header("Audio")]
    public AudioSource turnOffLaserSound;

    void Start()
    {
        SetLaserState(true);
    }

    //turn off laser function for when the button is pressed
    public void TurnOffLaser()
    {
        StartCoroutine(WaitForButtonSound());
    }
    
    //stalls so that laser turn off sound and turn off action plays after button 
    // - shows better feedback of what button did  
    private IEnumerator WaitForButtonSound()
    {
        yield return new WaitForSeconds(0.5f);
        SetLaserState(false);
        turnOffLaserSound.Play();
    }
}
