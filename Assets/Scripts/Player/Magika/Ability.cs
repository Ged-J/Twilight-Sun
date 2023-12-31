using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Ability : MonoBehaviour
{
    public AbilityData data;
    public GameObject explosionGO;


    private CircleCollider2D collider;
    private Rigidbody2D rb;
    private Vector3 shootDir;
    private int spellSlot;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        collider.isTrigger = true;
        collider.radius = data.spellRadius;

        rb.isKinematic = true;

        //Destroy(this.gameObject, data.lifeTime);
        StartCoroutine(DestroyAbility());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy" || collision.GetType() == typeof(CircleCollider2D))
        {
            return;
        } // if on midground do something
        //do emission stuff

        if (spellSlot != 0)
        {
            if (collision.tag == "Enemy")
            {
                EnemyController enemy = collision.GetComponent<EnemyController>();
                enemy.TakeDamage(data.baseDamage); // sub this with mutliplier method if its added
                Instantiate(explosionGO, transform.position, transform.rotation);
            }

            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (spellSlot == 0) return;
        transform.position += shootDir * data.projectileSpeed * Time.deltaTime;
    }

    public void Setup(Vector3 shootDir, int spellSlot)
    {
        this.shootDir = shootDir;
        this.spellSlot = spellSlot;
    }

    IEnumerator DestroyAbility()
    {
        yield return new WaitForSeconds(data.lifeTime);
        DisableAbility();
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
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
}