using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager alreadyPlaying;
    //avoids playing double audio when we reload same level
    //makes the music manager global, if it already exists we stop and play again

    void Awake()
    {
        if (alreadyPlaying != null)
        {
            Destroy(gameObject);
        }
        else
        {
            alreadyPlaying = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
