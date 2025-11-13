using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = GetComponent<AudioSource>();
        
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null)
        {
            return;
        }
        
        sfxSource.PlayOneShot(clip, volume);
    }
}