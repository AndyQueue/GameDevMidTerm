using UnityEngine;

public class GreenLaser : Laser
{
    void Start()
    {
        SetLaserState(true);
    }

    public void TurnOffLaser()
    {
        SetLaserState(false);
    }
}
