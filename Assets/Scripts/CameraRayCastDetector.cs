using System.Collections;
using UnityEngine;

public class CameraRayCastDetector : MonoBehaviour, IDetector
{
    private GameObject player;
    private Vector2 playerPos;
    [SerializeField] public float cameraViewRadius = 50f;
    [SerializeField] public float cameraViewAngle = 90f;
    [SerializeField] public LayerMask obstacleMask;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


    }

    void Update()
    {
        playerPos = player.transform.position;
        if (CanSeePlayer(playerPos))
        {
            OnPlayerDetected(player.GetComponent<PlayerDies>());
        }
    }

    public void OnPlayerDetected(PlayerDies player)
    {
        Debug.Log("Player Detected by Camera");
        player?.Dies();

    }

    bool CanSeePlayer(Vector2 playerPos)
    {
        Vector2 dirToPlayer = playerPos - (Vector2)transform.position;

        if (dirToPlayer.magnitude > cameraViewRadius)
        {
            Debug.Log("Player is too far outside of camera view radius: " + dirToPlayer.magnitude + " units");
            return false;
        }

        float angle = Vector2.Angle(transform.up, dirToPlayer);

        if (angle < 180 - (cameraViewAngle / 2f) || angle > 180 + (cameraViewAngle / 2f))
        {
            Debug.Log("Player is outside of camera view angle: " + angle + " degrees");
            return false;
        }


        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, cameraViewRadius, obstacleMask);
        Debug.DrawRay(transform.position, dirToPlayer, Color.green);
        Debug.Log("Raycasting from " + transform.position + " to " + playerPos + ", hit: " + (hit.collider != null ? hit.collider.name : "nothing"));

        // If the ray hits nothing (or hits the player), player is visible
        return hit.collider == null || hit.collider.CompareTag("Player");
    }

}
