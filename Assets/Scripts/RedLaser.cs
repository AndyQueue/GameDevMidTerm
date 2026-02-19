using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedLaser : MonoBehaviour, IDetector
{
    public float onTime;
    public float offTime;

    private bool laserOn = true;
    private SpriteRenderer spriteRenderer;
    private Collider2D laserCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        laserCollider = GetComponent<Collider2D>();
        StartCoroutine(LaserTimer());
    }

    private IEnumerator LaserTimer()
    {
        while (true)
        {
            laserOn = !laserOn;

            spriteRenderer.enabled = laserOn;
            laserCollider.enabled = laserOn;
            //enables physics and visual of laser

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
        if (laserOn && other.CompareTag("Player"))
        {
            PlayerDies player = other.GetComponent<PlayerDies>();
            OnPlayerDetected(player);

        }

    }
}
