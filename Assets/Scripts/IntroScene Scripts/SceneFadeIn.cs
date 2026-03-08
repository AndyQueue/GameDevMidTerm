using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    private SceneFader sceneFader;
    private Image fadeImage;

    void Awake()
    {
        sceneFader = GetComponent<SceneFader>();
        fadeImage = GetComponent<Image>();
    }

    void Start()
    {
        StartCoroutine(sceneFader.FadeOut(2.5f, fadeImage));
    }

}