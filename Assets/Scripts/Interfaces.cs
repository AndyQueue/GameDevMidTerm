using UnityEngine;

public interface IDetector
{
    void OnPlayerDetected(PlayerDies player); 
    //when player is detected by a dectector the player is caught
}

public interface IHidable
{
    void Hide();
    void Unhide();
}