using UnityEngine;

public interface IDetector
{
    void OnPlayerDetected(PlayerCaught player); 
    //when player is detected by a dectector the player is caught
}

public interface IHidable
{
    void Hide();
    void Unhide();
}