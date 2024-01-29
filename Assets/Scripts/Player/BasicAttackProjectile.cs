using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BasicAttackProjectile : MonoBehaviour
{
    public float projectileSpeed = 1f;
    private Vector3 shootDir;
    public GameObject explosionGO;
    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
    }

    private void Update()
    {
        transform.position += shootDir * projectileSpeed * Time.deltaTime;
    }

    IEnumerator DestroyAbility()
    {
        yield return new WaitForSeconds(5);
        DisableAbility();
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        StartCoroutine(DestroyAbility());
    }

    private void DisableAbility()
    {
        ParticleSystem[] ps;
        ps = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem childPS in ps)
        {
            ParticleSystem.EmissionModule childPSEmissionModule = childPS.emission;
            childPSEmissionModule.enabled = false;
        }

        if (gameObject.GetComponentInChildren<AudioSource>() != null)
        {
            StartCoroutine(FadeAudio(gameObject.GetComponentInChildren<AudioSource>(), 1, 0));
        }

        if (gameObject.GetComponentInChildren<Light2D>() != null)
        {
            gameObject.GetComponentInChildren<Light2D>().intensity = 0;
        }
    }

    IEnumerator FadeAudio(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy" || collision.GetType() == typeof(CircleCollider2D))
            return;

        EnemyController enemy = collision.GetComponent<EnemyController>();
        enemy.TakeDamage(5);
        var newExplosion = Instantiate(explosionGO, transform.position, transform.rotation); //needs audio source
        Destroy(newExplosion, 5);
        Destroy(this.gameObject);
    }
}

