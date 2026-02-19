using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedLaser : MonoBehaviour, IDetector
{
    public GameObject laserPrefab;
    public float onTime;
    public float offTime;

    private bool laserOn = true;

    void Start()
    {
        StartCoroutine(TurnOnOffLaser());
    }

    private IEnumerator TurnOnOffLaser()
    {
        while (true)
        {
            laserOn = !laserOn;
            laserPrefab.SetActive(laserOn);
            //SetActive activates and deactivates the laser based on the boolean

            if (laserOn)
            {
                yield return new WaitForSeconds(onTime);
            }
            else
            {
                yield return new WaitForSeconds(offTime);
            }
        }
    }
    public void OnPlayerDetected(PlayerDies player)
    {
        if (laserOn)
        {
            {
                Debug.Log("Player Detected by Camera");
                player?.Dies();
            }
        }
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
