using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController instance = null;
    public bool musicEnabled = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StopMusic()
    {
        // Stop the music from playing
        gameObject.SetActive(false);

        musicEnabled = false;
    }

    public void PlayMusic()
    {
        gameObject.SetActive(true);

        musicEnabled = true;
    }
}
