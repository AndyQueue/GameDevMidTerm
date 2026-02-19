using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraDetector : MonoBehaviour, IDetector
{

    void Start()
    {
    }

    public void OnPlayerDetected(PlayerDies player)
    {
        Debug.Log("Player Detected by Camera");
        player?.Dies();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDies player = other.GetComponent<PlayerDies>();
            OnPlayerDetected(player);

        }
    }
}
