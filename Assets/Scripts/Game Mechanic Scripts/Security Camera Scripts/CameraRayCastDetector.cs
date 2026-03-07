using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraRayCastDetector : MonoBehaviour, IDetector
{
    [SerializeField] public LayerMask obstacleMask; //layer for obstacles that will be detected by camera raycast

    private GameObject player;
    private Vector2 playerPos;
    private float cameraViewRadius; // distance from camera at which player can be detected
    private float cameraViewAngle; // angle form camera at which player can be detected
    private float playerWidth;
    private float playerHeight;
    private Vector2 rayOrigin;

    private Light2D cameraLight; // 2D Light object that visualizes the camera field of vision

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rayOrigin = (Vector2)transform.position; // same as the center of the camera
        playerWidth = player.GetComponent<Collider2D>().bounds.extents.x;
        playerHeight = player.GetComponent<Collider2D>().bounds.extents.y;

        cameraLight = GetComponentInChildren<Light2D>();
        SyncCameraToLight();
    }

    void Update()
    {
        playerPos = player.transform.position;
        if (CanSeePlayer(playerPos))
        {
            OnPlayerDetected(player.GetComponent<PlayerCaught>());
        }
    }

    void SyncCameraToLight()
    // Sync the camera's view radius and angle to match the Light2D's properties
    // So that when building scenes, camera's fucntionality changes to match to the visual properties of the light.
    {
        if (cameraLight == null)
        {
            Debug.LogWarning("CameraLight: Missing child Light2D component.");
            return;
        }
       ;
        // sets outer values, adjust inner values in unity to what visually looks good
        cameraViewRadius = cameraLight.pointLightOuterRadius;
        cameraViewAngle = cameraLight.pointLightOuterAngle;
    }

    public void OnPlayerDetected(PlayerCaught player)
    {
        Debug.Log("Player Detected by Camera");
        player?.Caught();

    }

    bool CanSeePlayer(Vector2 playerPos)
    {
        if (player.GetComponent<PlayerMovement>().IsHiding()) { return false; }

        // Define ray targets at the center and 4 edges of the player's collider
        Vector2[] rayTargets = new Vector2[]
        {
            playerPos, // Center
            playerPos + new Vector2(playerWidth, 0), // Right edge
            playerPos - new Vector2(playerWidth, 0), // Left edge
            playerPos + new Vector2(0, playerHeight), // Top edge
            playerPos - new Vector2(0, playerHeight)  // Bottom edge
        };


        if (!IsPlayerInCameraRadius(rayTargets))
        {
            // Debug.Log("Player is outside of camera view radius");
            return false;
        }

        if (!IsPlayerInCameraAngle(rayTargets))
        {
            // Debug.Log("Player is outside of camera view angle");
            return false;
        }

        // raycast to each target point to check for line of sight
        foreach (Vector2 target in rayTargets)
        {
            Vector2 dir = target - (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, cameraViewRadius, obstacleMask);
            Debug.DrawRay(transform.position, dir, Color.green);
            // Debug.Log("Raycasting from " + transform.position + " to " + target + ", hit: " + (hit.collider != null ? hit.collider.name : "nothing"));

            if (hit.collider == null || hit.collider.CompareTag("Player"))
            {
                return true; // Player is visible through this ray
            }
        }
        return false; // Player is not visible through any ray
    }

    private bool IsPlayerInCameraRadius(Vector2[] rayTargets)
    {
        foreach (Vector2 target in rayTargets)
        {
            Vector2 dirToPlayer = target - rayOrigin;
            if (dirToPlayer.magnitude < cameraViewRadius)
            {
                return true; // At least one target is within the camera view radius
            }
        }
        return false; // No targets are within the camera view radius

    }

    private bool IsPlayerInCameraAngle(Vector2[] rayTargets)
    {
        foreach (Vector2 target in rayTargets)
        {
            Vector2 dirToPlayer = target - rayOrigin;
            float angle = Vector2.Angle(transform.up, dirToPlayer);
            if (angle > 180 - (cameraViewAngle / 2f) && angle < 180 + (cameraViewAngle / 2f))
            {
                return true; // At least one target is within the camera view angle
            }
        }
        return false; // No targets are within the camera view angle
    }

}
