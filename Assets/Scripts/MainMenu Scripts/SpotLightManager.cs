using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpotLightManager : MonoBehaviour
{
    private Light2D Spotlight;
    [SerializeField] private float lightYPosition = 1.9f;
    [SerializeField] private float intensity = 0.35f;


    void Start()
    {
        Spotlight = GetComponent<Light2D>();
        disableSpotLight();

    }

    public void moveSpotLight(int levelNumber)
    {
        Debug.Log("Moving SpotLight to level " + levelNumber);
        float zRotation = (levelNumber - 3) * 3f; // Rotate more for levels further from the center
        Spotlight.transform.position = new Vector3((levelNumber - 3) * 3.15f, lightYPosition, 0f);
        Spotlight.transform.rotation = Quaternion.Euler(0f, 0f, 180 + zRotation);

    }

    public void enableSpotLight()
    {
        Debug.Log("Enabling SpotLight");
        Spotlight.intensity = intensity;

    }

    public void disableSpotLight()
    {
        Debug.Log("Disabling SpotLight");
        Spotlight.intensity = 0f;
    }
}
