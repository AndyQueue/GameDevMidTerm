using System.Collections;
using UnityEngine;

public class BlueLaser : Laser
{
    //blue laser turns on and off
    public float onTime;
    public float offTime;

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
            yield return new WaitForSeconds(onTime);
            //turn laser off
            SetLaserState(false);
            yield return new WaitForSeconds(offTime);
        }
    }
}
