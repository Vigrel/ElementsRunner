using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController instance = null;
    private static AudioSource audioSource;
    public bool musicEnabled = true;

    // Static reference to the instance
    public static MusicController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicController>();

                // If there is no instance in the scene, create a new one
                if (instance == null)
                {
                    GameObject obj = new GameObject("MusicController");
                    instance = obj.AddComponent<MusicController>();
                }

                // Ensure that the instance is not destroyed when loading a new scene
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    void Awake()
    {
        // If an instance already exists and it's not this one, destroy this GameObject
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance and mark it as persistent between scene changes
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void StopMusic()
    {
        // Stop the music from playing
        audioSource.Stop();
        musicEnabled = false;
    }

    public void PlayMusic()
    {
        audioSource.Play();
        musicEnabled = true;
    }
}
