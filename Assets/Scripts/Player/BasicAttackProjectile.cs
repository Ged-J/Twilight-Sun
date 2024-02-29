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
        // Check if the collision is with an enemy and has the correct collider type
        if (collision.tag == "Enemy" && collision.GetType() != typeof(CircleCollider2D))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null) // Ensure there is an EnemyController to call TakeDamage on
            {
                enemy.TakeDamage(5);
            }

            TriggerExplosion();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Midground")) // Check if the collision is with a wall
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        var newExplosion = Instantiate(explosionGO, transform.position, Quaternion.identity); // Instantiate the explosion effect
        Destroy(newExplosion, 5); // Destroy the explosion GameObject after 5 seconds to clean up
        Destroy(this.gameObject); // Destroy the fireball
    }

}

