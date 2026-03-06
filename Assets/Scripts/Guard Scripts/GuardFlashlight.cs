using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GuardFlashlight : MonoBehaviour
{
    // This script renders a flashlight to show the guard's "sight" within which the player will get caught. The light is rendered based on the guard's raycast dimensions.
    private Guard guard;

    private Light2D Flashlight;
    private float viewLength;
    private float viewHeight;
    private float viewAngle;

    void Start()
    {
        guard = GetComponentInParent<Guard>();
        Flashlight = GetComponent<Light2D>();
        if (Flashlight == null)
        {
            Debug.LogError("GuardFlashlight: No Light2D component found on flashlight object.");
        }

        viewLength = guard.GetSightWidth();

        viewAngle = CalculateLightAngle(guard.GetSightHeight());


        SyncLightToGuardRange();

    }
    float CalculateLightAngle(float guardSightHeight)
    {
        // written using Copilot
        // Calculate the angle based on the height of the guard's collider and the view length
        // Using basic trigonometry: tan(angle) = opposite/adjacent = (guardHeight/2) / viewLength

        float angleRad = Mathf.Atan(((guardSightHeight + 5f) / 2f) / viewLength); //offsetting guard height by 5f to account for flashlight being lower than the gaurd's center
        return angleRad * Mathf.Rad2Deg; // Convert to degrees
    }

    void SyncLightToGuardRange()
    // Sync the light's radius and angle to match the guard's raycast properties
    {
        Flashlight.pointLightOuterRadius = viewLength;
        Flashlight.pointLightInnerRadius = 1f;

        Flashlight.pointLightOuterAngle = viewAngle;
        Flashlight.pointLightInnerAngle = 15f;
    }

}