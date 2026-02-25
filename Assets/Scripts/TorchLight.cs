using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class TorchLight : MonoBehaviour
{

    public float flickerSpeed = 1f;

    public float baseIntensity = 0.4f;
    public float intensityAmplitude = 0.15f;

    public float baseRadius = 1.75f;
    public float radiusAmplitude = 0.15f;

    private Light2D torchLight;

    void Start()
    {
        torchLight = GetComponent<Light2D>();
        flickerSpeed = Random.Range(flickerSpeed - 0.5f, flickerSpeed + 0.5f); // Randomize flicker speed for more natural effect

    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);

        torchLight.intensity = baseIntensity + (noise - 0.5f) * 2f * intensityAmplitude;
        torchLight.pointLightOuterRadius = baseRadius + (noise - 0.5f) * 2f * radiusAmplitude;

    }
}
