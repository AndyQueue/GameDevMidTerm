using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedLaser : Laser
{
    void Start()
    {
        SetLaserState(true);
    }
}