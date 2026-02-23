using System.Collections;
using UnityEngine;

public class BlueLaser : Laser
{
    public float onTime;
    public float offTime;

    void Start()
    {
        StartCoroutine(LaserTimer());
    }

    private IEnumerator LaserTimer()
    {
        while (true)
        {
            SetLaserState(true);
            yield return new WaitForSeconds(onTime);
            
            SetLaserState(false);
            yield return new WaitForSeconds(offTime);
        }
    }
}
