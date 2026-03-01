using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager alreadyPlaying;
    //avoids playing double audio when we reload same level
    //static makes the music manager global, if it already exists do not start playing a new audio
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
            //keeps old music manager which is why we have to check if it exists when loading new scene
        }
    }
}
