using UnityEngine;

public interface IDetector
{
    void OnPlayerDetected(PlayerCaught player);
}

public interface IHidable
{
    void Hide();
    void Unhide();
}