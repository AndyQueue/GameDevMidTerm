using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraColliderDetector : MonoBehaviour, IDetector
{
    // Old script using a triangle collider to handle detections, now using CameraRayCastDetector

    public void OnPlayerDetected(PlayerCaught player)
    {
        Debug.Log("Player Detected by Camera");
        player?.Caught();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCaught player = other.GetComponent<PlayerCaught>();
            OnPlayerDetected(player);

        }
    }
}
