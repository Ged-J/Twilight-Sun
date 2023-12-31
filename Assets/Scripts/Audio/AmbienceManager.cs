using System.Collections;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    private AudioClip _audioClip;
    [SerializeField]public  AudioClip[] BGS;
    private static AudioClip[] BGSstatic;
    private static AudioSource audioSource;
    private static AmbienceManager instance;

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
    }
    
    private void Start()
    {
        BGSstatic = BGS;
        audioSource = GetComponent<AudioSource>();
    }

    public static void SetAmbience(string bgsName)
    {
        foreach (var clip in BGSstatic)
        {
            if(clip.name == bgsName)
                audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public static void SetNone()
    {
        if (audioSource != null) { 
            audioSource.Stop();
        }
    }

    public static IEnumerator FadeOut(float fadeTime, float endVolume) {
        var startVolume = audioSource.volume;
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