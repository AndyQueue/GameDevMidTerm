using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedLaser : Laser
{
    //red laser just turns on and stays on
    void Start()
    {
        SetLaserState(true);
    }
}