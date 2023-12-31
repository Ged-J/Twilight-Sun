using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class SFXManager : MonoBehaviour
{
    private AudioClip _audioClip;
    private static AudioSource audioSource;
    private static SFXManager instance;

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
        audioSource = GetComponent<AudioSource>();
    }
    
    public static void PlaySFX(string sfxName, float pitch = 1f, float volume = 1f)
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/SFX/" + sfxName));
        audioSource.volume = volume;
        audioSource.pitch = pitch;
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