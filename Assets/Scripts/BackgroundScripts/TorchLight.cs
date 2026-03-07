using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class TorchLight : MonoBehaviour
{
    // Creates a flickering effect for a torch lights in the background for visual atmospheric effect.

    public float flickerSpeed = 1.8f;

    public float baseIntensity = 0.3f;
    public float intensityAmplitude = 0.2f;

    public float baseRadius = 1.75f;
    public float radiusAmplitude = 0.2f;

    private Light2D torchLight;

    void Start()
    {
        torchLight = GetComponent<Light2D>();
        flickerSpeed = Random.Range(flickerSpeed - 0.5f, flickerSpeed + 0.5f);

    }

    void Update()
    {
        // written using AI
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);

        torchLight.intensity = baseIntensity + (noise - 0.5f) * 2f * intensityAmplitude;
        torchLight.pointLightOuterRadius = baseRadius + (noise - 0.5f) * 2f * radiusAmplitude;

    }
}
