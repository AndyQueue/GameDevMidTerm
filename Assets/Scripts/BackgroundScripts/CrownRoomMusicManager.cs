//AI helped to write this script for making the music fade out into new music

using UnityEngine;
using System.Collections;

public class CrownRoomMusicManager : MonoBehaviour
{
    public static CrownRoomMusicManager instance;
    public AudioSource bgMusic;
    public float fadeDuration = 1.5f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(FadeOutOldAndFadeInNew());
    }

    private IEnumerator FadeOutOldAndFadeInNew()
    {
        // fade out old music
        if (MusicManager.alreadyPlaying != null)
        {
            AudioSource oldMusic = MusicManager.alreadyPlaying.GetComponent<AudioSource>();
            float startVolume = oldMusic.volume;
            while (oldMusic.volume > 0)
            {
                oldMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }
            MusicManager.Stop();
        }

        // fade in new music
        bgMusic.volume = 0;
        bgMusic.Play();
        while (bgMusic.volume < 1)
        {
            bgMusic.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}