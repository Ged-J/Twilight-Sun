using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject newHitEffect;
    PlayerController player;
    int damage;
    float projectileSpeed = 1.5f;
    Vector3 shootDir;
    bool isRanged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // This block will execute if the projectile hits the player
            print("you got got");
            player.TakeDamage(damage);
            print(player.currentHealth);
            TriggerHitEffect();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Midground"))
        {
            // This block will execute if the projectile hits the midground/wall
            TriggerHitEffect();
        }
    }

    private void TriggerHitEffect()
    {
        newHitEffect = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(newHitEffect, 3);
        Destroy(gameObject);
    }

    public void Setup(int damage, Vector3 shootDir, bool isRanged)
    {
        player = FindObjectOfType<PlayerController>();
        this.damage = damage;
        this.shootDir = shootDir;
        this.isRanged = isRanged;
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        if (!isRanged)
            return;

        transform.position += shootDir * projectileSpeed * Time.deltaTime;
    }
}