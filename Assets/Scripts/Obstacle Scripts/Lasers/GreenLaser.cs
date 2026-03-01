using UnityEngine;

public class GreenLaser : Laser
{
    void Start()
    {
        SetLaserState(true);
    }

    //has a turn off laser function for when the button is pressed
    public void TurnOffLaser()
    {
        SetLaserState(false);
    }
}
