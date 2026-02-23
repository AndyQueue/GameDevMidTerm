using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class CameraRayCastDetector : MonoBehaviour, IDetector
{
    private GameObject player;
    private Vector2 playerPos;
    private float cameraViewRadius;
    private float cameraViewAngle;

    // [SerializeField] public float cameraViewRadius = 50f;
    // [SerializeField] public float cameraViewAngle = 90f;
    [SerializeField] public LayerMask obstacleMask;


    private Light2D cameraLight;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraLight = GetComponentInChildren<Light2D>();
        SyncCameraToLight();
    }

    void Update()
    {
        playerPos = player.transform.position;
        if (CanSeePlayer(playerPos))
        {
            OnPlayerDetected(player.GetComponent<PlayerDies>());
        }
    }

    void SyncCameraToLight()
    // Sync the camera's view radius and angle to match the Light2D's properties
    {
        if (cameraLight == null)
        {
            Debug.LogWarning("CameraLight: Missing child Light2D component.");
            return;
        }
       ;

        cameraViewRadius = cameraLight.pointLightInnerRadius;
        cameraViewAngle = cameraLight.pointLightInnerAngle;
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
            // Debug.Log("Player is too far outside of camera view radius: " + dirToPlayer.magnitude + " units");
            return false;
        }

        float angle = Vector2.Angle(transform.up, dirToPlayer);

        if (angle < 180 - (cameraViewAngle / 2f) || angle > 180 + (cameraViewAngle / 2f))
        {
            // Debug.Log("Player is outside of camera view angle: " + angle + " degrees");
            return false;
        }


        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, cameraViewRadius, obstacleMask);
        Debug.DrawRay(transform.position, dirToPlayer, Color.green);
        // Debug.Log("Raycasting from " + transform.position + " to " + playerPos + ", hit: " + (hit.collider != null ? hit.collider.name : "nothing"));

        // If the ray hits nothing (or hits the player), player is visible
        return hit.collider == null || hit.collider.CompareTag("Player");
    }



}
