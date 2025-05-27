using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (audioSource.clip == clip) return; 

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
