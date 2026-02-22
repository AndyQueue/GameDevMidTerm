using UnityEngine;

public class LaserButton : MonoBehaviour
{
    public GreenLaser targetLaser;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetLaser != null)
            {
                targetLaser.TurnOffLaser();
                //maybe turn color or something - also maybe quick pressing animation
            }
        }
    }
}