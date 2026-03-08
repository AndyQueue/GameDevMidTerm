using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager alreadyPlaying;
    
    void Awake()
    {
        if (alreadyPlaying != null)
        {
            Destroy(gameObject);
            //dont play new music if we already have one playing
            //destroys music we try to spawn at start of scene
        }
        else
        {
            alreadyPlaying = this;
            DontDestroyOnLoad(gameObject);
            //doesnt stop music when loading new scene
            //keeps old music manager
        }
    }

    // stopping music when needed
    public static void Stop()
    {
        if (alreadyPlaying != null)
        {
            Destroy(alreadyPlaying.gameObject);
            alreadyPlaying = null;
        }
    }
}
