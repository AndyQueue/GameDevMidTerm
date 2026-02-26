using UnityEngine;

public interface IDetector
{
    void OnPlayerDetected(PlayerDies player);
}

public interface IHidable
{
    void Hide();
    void Unhide();
}