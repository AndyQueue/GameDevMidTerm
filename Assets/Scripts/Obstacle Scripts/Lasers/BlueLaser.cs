using System.Collections;
using UnityEngine;

public class BlueLaser : Laser
{
    //blue laser turns on and off
    [Header("Laser Timing")]
    public float onTime;
    public float offTime;

    //[Header("Audio")]
    //public AudioSource turnOnLaserSound;
    //public AudioSource turnOffLaserSound;

    void Start()
    {
        //uses coroutine for continuous on and off timing
        StartCoroutine(LaserTimer());
    }

    private IEnumerator LaserTimer()
    {
        while (true)
        {
            //turn laser on
            SetLaserState(true);
            //turnOnLaserSound.PlayOneShot();
            yield return new WaitForSeconds(onTime);
            
            //turn laser off
            SetLaserState(false);
            //turnOffLaserSound.PlayOneShot();
            yield return new WaitForSeconds(offTime);
        }
    }
}
