using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static AudioSource audioSource;
    private static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    public static float GetMusicPosition()
    {
        return audioSource.time;
    }
    
    public static AudioClip GetCurrentMusic()
    {
        return audioSource.clip;
    }
    
    public static void SetMusic(string i)
    {
        audioSource.clip = Resources.Load<AudioClip>("Sounds/BGM/" + i);
        audioSource.Play();
    }

    public static void Play()
    {
        audioSource.Play();
    }
    
    public static void SetNone()
    {
        audioSource.clip = null;
    }
        
    public static void SetVolume(float vol)
    {
        if (audioSource.clip != null)
        {
            audioSource.volume = vol;
        }
    }
    
    public static IEnumerator FadeOut(float fadeTime, float endVolume) {
        float startVolume = audioSource.volume;
        while (audioSource.volume > endVolume) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(float fadeTime, float endVolume) {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < endVolume) {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    
	
    }

